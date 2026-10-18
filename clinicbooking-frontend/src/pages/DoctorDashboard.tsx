import { toast } from "sonner"
import { Button } from "@/components/ui/button.tsx"
import { useAuth } from "@/context/AuthProvider.tsx"
import { useAppointments } from "@/hooks/useAppointments.ts"
import { updateAppointment } from "@/api/appointments.ts"
import { UnauthorizedError } from "@/api/http.ts"
import type { Appointment, AppointmentStatus } from "@/schemas/appointment.ts"
import AppointmentCard from "@/components/appointments/AppointmentCard.tsx"
import Pagination from "@/components/common/Pagination.tsx"

export default function DoctorDashboard() {
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

    const handleStatusChange = async (appointment: Appointment, status: AppointmentStatus) => {
        if (!token) return
        if (status === "Cancelled" && !window.confirm("Cancel this appointment?")) return

        try {
            const updated = await updateAppointment(token, appointment.id, { status })
            toast.success(`Appointment ${updated.status.toLowerCase()}`)
            reload()
        } catch (error) {
            if (error instanceof UnauthorizedError) logoutUser()
            toast.error(error instanceof Error ? error.message : "Update failed")
        }
    }

    const handleNotesSave = async (appointment: Appointment, notes: string) => {
        if (!token) return false

        try {
            await updateAppointment(token, appointment.id, { notes })
            toast.success("Notes saved")
            reload()
            return true
        } catch (error) {
            if (error instanceof UnauthorizedError) logoutUser()
            toast.error(error instanceof Error ? error.message : "Could not save notes")
            return false
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
                        onStatusChange={handleStatusChange}
                        onNotesSave={handleNotesSave}
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