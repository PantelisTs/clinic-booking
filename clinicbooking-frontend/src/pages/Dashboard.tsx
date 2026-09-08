import { useAuth } from "@/context/AuthProvider.tsx"
import AdminDashboard from "@/pages/AdminDashboard.tsx"
import DoctorDashboard from "@/pages/DoctorDashboard.tsx"
import PatientDashboard from "@/pages/PatientDashboard.tsx"

function renderDashboardByRole(role: string | null) {
    switch (role) {
        case "ADMIN":
            return <AdminDashboard />
        case "DOCTOR":
            return <DoctorDashboard />
        case "PATIENT":
            return <PatientDashboard />
        default:
            return <p className="text-destructive">Unknown role: {role}</p>
    }
}

export default function Dashboard() {
    const { role } = useAuth()

    return <div className="p-8">{renderDashboardByRole(role)}</div>
}