import { Button } from "@/components/ui/button.tsx"
import { useAppointments } from "@/hooks/useAppointments.ts"
import AppointmentCard from "@/components/appointments/AppointmentCard.tsx"
import Pagination from "@/components/common/Pagination.tsx"

export default function AdminDashboard() {
    const {
        appointments,
        pageNumber,
        setPageNumber,
        totalPages,
        totalRecords,
        activeOnly,
        toggleActiveOnly,
        loading,
    } = useAppointments("all")

    return (
        <div>
            <div className="mb-1 flex items-center justify-between">
                <h2 className="text-xl font-semibold">All appointments</h2>
                <Button variant="outline" size="sm" onClick={toggleActiveOnly}>
                    {activeOnly ? "Show all" : "Show active only"}
                </Button>
            </div>
            <p className="text-sm text-muted-foreground mb-4">
                {totalRecords} {activeOnly ? "active" : "in total"}
            </p>

            {loading && <p className="text-muted-foreground">Loading...</p>}

            {!loading && appointments.length === 0 && (
                <p className="text-muted-foreground">
                    {activeOnly ? "There are no active appointments." : "There are no appointments yet."}
                </p>
            )}

            <div className="grid gap-4">
                {appointments.map((appointment) => (
                    <AppointmentCard key={appointment.id} appointment={appointment} />
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