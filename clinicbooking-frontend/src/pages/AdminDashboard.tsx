import { useEffect, useState } from "react"
import { toast } from "sonner"
import { useAuth } from "@/context/AuthProvider.tsx"
import { getAllAppointments } from "@/api/appointments.ts"
import { UnauthorizedError } from "@/api/http.ts"
import type { Appointment } from "@/schemas/appointment.ts"
import AppointmentCard from "@/components/appointments/AppointmentCard.tsx"
import Pagination from "@/components/common/Pagination.tsx"

const PAGE_SIZE = 10

export default function AdminDashboard() {
    const { token, logoutUser } = useAuth()
    const [appointments, setAppointments] = useState<Appointment[]>([])
    const [pageNumber, setPageNumber] = useState(1)
    const [totalPages, setTotalPages] = useState(1)
    const [totalRecords, setTotalRecords] = useState(0)
    const [loading, setLoading] = useState(true)


    useEffect(() => {
        if (!token) return

        getAllAppointments(token, pageNumber, PAGE_SIZE)
            .then((res) => {
                setAppointments(res.data)
                setTotalPages(res.totalPages)
                setTotalRecords(res.totalRecords)
            })
            .catch((error: unknown) => {
                if (error instanceof UnauthorizedError) logoutUser()
                toast.error(error instanceof Error ? error.message : "Request failed")
            })
            .finally(() => setLoading(false))
    }, [token, logoutUser, pageNumber])

    return (
        <div>
            <h2 className="text-xl font-semibold mb-1">All appointments</h2>
            <p className="text-sm text-muted-foreground mb-4">{totalRecords} in total</p>

            {loading && <p className="text-muted-foreground">Loading...</p>}

            {!loading && appointments.length === 0 && (
                <p className="text-muted-foreground">There are no appointments yet.</p>
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