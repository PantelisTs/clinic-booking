import { Button } from "@/components/ui/button.tsx"
import { useAuth } from "@/context/AuthProvider.tsx"
import { useNavigate } from "react-router"

export default function Dashboard() {
    const { username, role, logoutUser } = useAuth()
    const navigate = useNavigate()

    const handleLogout = () => {
        logoutUser()
        navigate("/login")
    }

    return (
        <div className="flex flex-col items-center justify-center min-h-screen gap-4">
            <p>
                Καλωσήρθες, {username} ({role})
            </p>
            <Button onClick={handleLogout}>Logout</Button>
        </div>
    )
}