import type { LoginFields } from "@/schemas/auth.ts"
import {createContext, type ReactNode, useContext, useState} from "react"
import { jwtDecode } from "jwt-decode"
import { deleteCookie, getCookie, setCookie } from "@/utils/cookies.ts"
import { login } from "@/api/auth.ts"

type AuthContextProps = {
    isAuthenticated: boolean
    token: string | null
    username: string | null
    role: string | null
    loginUser: (fields: LoginFields) => Promise<void>
    logoutUser: () => void
}

type JwtPayload = {
    username: string
    role: string
    exp: number
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined)

function readFromToken(token: string | null): JwtPayload | null {
    if (!token) return null
    try {
        const decoded = jwtDecode<JwtPayload>(token)
        if (decoded.exp * 1000 <= Date.now()) return null
        return decoded
    } catch {
        return null
    }
}

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const cookieToken = getCookie("access_token") ?? null
    const initialPayload = readFromToken(cookieToken)

    const [token, setToken] = useState<string | null>(() =>
        initialPayload ? cookieToken : null
    )
    const [payload, setPayload] = useState<JwtPayload | null>(() => initialPayload)

    const loginUser = async (fields: LoginFields) => {
        const res = await login(fields)
        setCookie("access_token", res.token, {
            expires: 1,
            SameSite: "Lax",
            secure: false,
            path: "/",
        })
        setToken(res.token)
        setPayload(readFromToken(res.token))
    }

    const logoutUser = () => {
        deleteCookie("access_token")
        setToken(null)
        setPayload(null)
    }

    return (
        <AuthContext.Provider
            value={{
                isAuthenticated: !!token,
                token,
                username: payload?.username ?? null,
                role: payload?.role ?? null,
                loginUser,
                logoutUser,
            }}
        >
            {children}
        </AuthContext.Provider>
    )
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
    const ctx = useContext(AuthContext)
    if (!ctx) throw new Error("useAuth must be used within an AuthProvider")
    return ctx
}