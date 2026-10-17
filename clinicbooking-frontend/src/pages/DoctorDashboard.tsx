import { useEffect, useState } from "react"
import { toast } from "sonner"
import { useAuth } from "@/context/AuthProvider.tsx"
import { getMyAppointments, updateAppointment } from "@/api/appointments.ts"
import { UnauthorizedError } from "@/api/http.ts"
import type { Appointment, AppointmentStatus } from "@/schemas/appointment.ts"
import AppointmentCard from "@/components/appointments/AppointmentCard.tsx"

export default function DoctorDashboard() {
    const { token, logoutUser } = useAuth()
    const [appointments, setAppointments] = useState<Appointment[]>([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        if (!token) return

        getMyAppointments(token)
            .then((res) => setAppointments(res.data))
            .catch((error: unknown) => {
                if (error instanceof UnauthorizedError) logoutUser()
                toast.error(error instanceof Error ? error.message : "Request failed")
            })
            .finally(() => setLoading(false))
    }, [token, logoutUser])

    const handleStatusChange = async (appointment: Appointment, status: AppointmentStatus) => {
        if (!token) return
        if (status === "Cancelled" && !window.confirm("Cancel this appointment?")) return

        try {
            const updated = await updateAppointment(token, appointment.id, { status })
            setAppointments((prev) => prev.map((a) => (a.id === updated.id ? updated : a)))
            toast.success(`Appointment ${updated.status.toLowerCase()}`)
        } catch (error) {
            if (error instanceof UnauthorizedError) logoutUser()
            toast.error(error instanceof Error ? error.message : "Update failed")
        }
    }

    return (
        <div>
            <h2 className="text-xl font-semibold mb-4">Your appointments</h2>

            {loading && <p className="text-muted-foreground">Loading...</p>}

            {!loading && appointments.length === 0 && (
                <p className="text-muted-foreground">You have no appointments yet.</p>
            )}

            <div className="grid gap-4">
                {appointments.map((appointment) => (
                    <AppointmentCard
                        key={appointment.id}
                        appointment={appointment}
                        onStatusChange={handleStatusChange}
                    />
                ))}
            </div>
        </div>
    )
}