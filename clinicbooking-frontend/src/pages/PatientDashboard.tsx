import { useEffect, useState } from "react"
import { toast } from "sonner"
import { useAuth } from "@/context/AuthProvider.tsx"
import { cancelAppointment, getMyAppointments } from "@/api/appointments.ts"
import { getDoctors } from "@/api/doctors.ts"
import { UnauthorizedError } from "@/api/http.ts"
import type { Appointment } from "@/schemas/appointment.ts"
import type { Doctor } from "@/schemas/doctor.ts"
import AppointmentCard from "@/components/appointments/AppointmentCard.tsx"
import BookAppointmentForm from "@/components/appointments/BookAppointmentForm.tsx"

export default function PatientDashboard() {
    const { token, logoutUser } = useAuth()
    const [appointments, setAppointments] = useState<Appointment[]>([])
    const [doctors, setDoctors] = useState<Doctor[]>([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        if (!token) return

        Promise.all([getMyAppointments(token), getDoctors(token)])
            .then(([appointmentsRes, doctorsRes]) => {
                setAppointments(appointmentsRes.data)
                setDoctors(doctorsRes.data)
            })
            .catch((error: unknown) => {
                if (error instanceof UnauthorizedError) logoutUser()
                toast.error(error instanceof Error ? error.message : "Request failed")
            })
            .finally(() => setLoading(false))
    }, [token, logoutUser])

    const handleBooked = (appointment: Appointment) => {
        setAppointments((prev) =>
            [...prev, appointment].sort((a, b) => b.scheduledAt.localeCompare(a.scheduledAt))
        )
    }

    const handleCancel = async (appointment: Appointment) => {
        if (!token) return
        if (!window.confirm("Cancel this appointment?")) return

        try {
            const updated = await cancelAppointment(token, appointment.id)
            setAppointments((prev) => prev.map((a) => (a.id === updated.id ? updated : a)))
            toast.success("Appointment cancelled")
        } catch (error) {
            if (error instanceof UnauthorizedError) logoutUser()
            toast.error(error instanceof Error ? error.message : "Cancel failed")
        }
    }

    return (
        <div>
            <h2 className="text-xl font-semibold mb-4">Your appointments</h2>

            <BookAppointmentForm doctors={doctors} onBooked={handleBooked} />

            {loading && <p className="text-muted-foreground">Loading...</p>}

            {!loading && appointments.length === 0 && (
                <p className="text-muted-foreground">You have no appointments yet.</p>
            )}

            <div className="grid gap-4">
                {appointments.map((appointment) => (
                    <AppointmentCard key={appointment.id} appointment={appointment} onCancel={handleCancel} />
                ))}
            </div>
        </div>
    )
}