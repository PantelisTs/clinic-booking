import { useEffect, useState } from "react"
import { toast } from "sonner"
import { useAuth } from "@/context/AuthProvider.tsx"
import { getAllAppointments, getMyAppointments } from "@/api/appointments.ts"
import { UnauthorizedError } from "@/api/http.ts"
import type { Appointment } from "@/schemas/appointment.ts"

const PAGE_SIZE = 10

export function useAppointments(scope: "my" | "all") {
    const { token, logoutUser } = useAuth()
    const [appointments, setAppointments] = useState<Appointment[]>([])
    const [pageNumber, setPageNumber] = useState(1)
    const [totalPages, setTotalPages] = useState(1)
    const [totalRecords, setTotalRecords] = useState(0)
    const [activeOnly, setActiveOnly] = useState(true)
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        if (!token) return

        const fetchPage = scope === "my" ? getMyAppointments : getAllAppointments

        fetchPage(token, pageNumber, PAGE_SIZE, activeOnly)
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
    }, [token, logoutUser, scope, pageNumber, activeOnly])

    const toggleActiveOnly = () => {
        setActiveOnly((prev) => !prev)
        setPageNumber(1)
    }

    return {
        appointments,
        setAppointments,
        pageNumber,
        setPageNumber,
        totalPages,
        totalRecords,
        activeOnly,
        toggleActiveOnly,
        loading,
    }
}