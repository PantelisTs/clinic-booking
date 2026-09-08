import { Link, useNavigate } from "react-router"
import { Button } from "@/components/ui/button.tsx"
import { useAuth } from "@/context/AuthProvider.tsx"

export default function Header() {
    const { isAuthenticated, username, logoutUser } = useAuth()
    const navigate = useNavigate()

    const handleLogout = () => {
        logoutUser()
        navigate("/login")
    }

    return (
        <header className="flex items-center justify-between border-b px-6 py-4">
            <Link to="/" className="font-semibold">
                Clinic Booking
            </Link>

            {isAuthenticated ? (
                <div className="flex items-center gap-4">
                    <span className="text-sm text-muted-foreground">{username}</span>
                    <Button variant="outline" onClick={handleLogout}>
                        Logout
                    </Button>
                </div>
            ) : (
                <div className="flex items-center gap-2">
                    <Link to="/login">
                        <Button variant="outline">Login</Button>
                    </Link>
                    <Link to="/register">
                        <Button>Sign up</Button>
                    </Link>
                </div>
            )}
        </header>
    )
}