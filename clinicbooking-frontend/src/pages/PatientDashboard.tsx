import { useEffect, useState } from "react"
import { toast } from "sonner"
import { Button } from "@/components/ui/button.tsx"
import { useAuth } from "@/context/AuthProvider.tsx"
import { useAppointments } from "@/hooks/useAppointments.ts"
import { cancelAppointment } from "@/api/appointments.ts"
import { getDoctors } from "@/api/doctors.ts"
import { UnauthorizedError } from "@/api/http.ts"
import type { Appointment } from "@/schemas/appointment.ts"
import type { Doctor } from "@/schemas/doctor.ts"
import AppointmentCard from "@/components/appointments/AppointmentCard.tsx"
import BookAppointmentForm from "@/components/appointments/BookAppointmentForm.tsx"
import Pagination from "@/components/common/Pagination.tsx"

export default function PatientDashboard() {
    const { token, logoutUser } = useAuth()
    const {
        appointments,
        pageNumber,
        setPageNumber,
        totalPages,
        activeOnly,
        toggleActiveOnly,
        reload,
        loading,
    } = useAppointments("my")
    const [doctors, setDoctors] = useState<Doctor[]>([])

    useEffect(() => {
        if (!token) return

        getDoctors(token)
            .then((res) => setDoctors(res.data))
            .catch((error: unknown) => {
                if (error instanceof UnauthorizedError) logoutUser()
                toast.error(error instanceof Error ? error.message : "Could not load doctors")
            })
    }, [token, logoutUser])

    const handleCancel = async (appointment: Appointment) => {
        if (!token) return
        if (!window.confirm("Cancel this appointment?")) return

        try {
            await cancelAppointment(token, appointment.id)
            toast.success("Appointment cancelled")
            reload()
        } catch (error) {
            if (error instanceof UnauthorizedError) logoutUser()
            toast.error(error instanceof Error ? error.message : "Cancel failed")
        }
    }

    return (
        <div>
            <div className="mb-4 flex items-center justify-between">
                <h2 className="text-xl font-semibold">Your appointments</h2>
                <Button variant="outline" size="sm" onClick={toggleActiveOnly}>
                    {activeOnly ? "Show all" : "Show active only"}
                </Button>
            </div>

            <BookAppointmentForm doctors={doctors} onBooked={reload} />

            {loading && <p className="text-muted-foreground">Loading...</p>}

            {!loading && appointments.length === 0 && (
                <p className="text-muted-foreground">
                    {activeOnly ? "You have no active appointments." : "You have no appointments yet."}
                </p>
            )}

            <div className="grid gap-4">
                {appointments.map((appointment) => (
                    <AppointmentCard
                        key={appointment.id}
                        appointment={appointment}
                        onCancel={handleCancel}
                    />
                ))}
            </div>

            <Pagination
                pageNumber={pageNumber}
                totalPages={totalPages}
                onPageChange={setPageNumber}
            />
        </div>
    )
}