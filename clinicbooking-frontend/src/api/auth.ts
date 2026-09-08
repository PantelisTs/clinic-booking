import type {
    LoginFields,
    LoginResponse,
    DoctorSignupFields,
    PatientSignupFields,
    SignupResponse,
} from "@/schemas/auth.ts"

const API_URL = import.meta.env.VITE_API_URL

function extractErrorMessage(data: unknown): string {
    if (data && typeof data === "object") {
        if ("detail" in data && typeof data.detail === "string") {
            return data.detail === "Bad Credentials"
                ? "Incorrect username or password"
                : data.detail
        }
        if ("errors" in data && data.errors && typeof data.errors === "object") {
            const firstField = Object.values(data.errors)[0]
            if (Array.isArray(firstField) && typeof firstField[0] === "string") {
                const message = firstField[0]
                return message.startsWith("Password must contain")
                    ? "Password must be 8+ characters with upper/lowercase, a number and a symbol"
                    : message
            }
        }
    }
    return "Request failed"
}

async function postJson<T>(path: string, body: unknown): Promise<T> {
    const res = await fetch(API_URL + path, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
    })

    if (!res.ok) {
        let message = "Request failed"
        try {
            const data = await res.json()
            message = extractErrorMessage(data)
        } catch (error) {
            console.error("Error parsing error response", error)
        }
        throw new Error(message)
    }

    return await res.json()
}

export function login(fields: LoginFields) {
    return postJson<LoginResponse>("/auth/login", fields)
}

export function registerDoctor(fields: DoctorSignupFields) {
    return postJson<SignupResponse>("/auth/register/doctor", { ...fields, roleId: 2 })
}

export function registerPatient(fields: PatientSignupFields) {
    return postJson<SignupResponse>("/auth/register/patient", { ...fields, roleId: 3 })
}