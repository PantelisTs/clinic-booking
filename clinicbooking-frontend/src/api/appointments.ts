import { request } from "@/api/http.ts"
import type { Appointment, PaginatedResult } from "@/schemas/appointment.ts"

export function getMyAppointments(token: string, pageNumber = 1, pageSize = 10) {
    return request<PaginatedResult<Appointment>>(
        "GET",
        `/appointments/my?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        { token }
    )
}