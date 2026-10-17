import { request } from "@/api/http.ts"
import type {Appointment, BookAppointmentFields, PaginatedResult} from "@/schemas/appointment.ts"

export function getMyAppointments(token: string, pageNumber = 1, pageSize = 10) {
    return request<PaginatedResult<Appointment>>(
        "GET",
        `/appointments/my?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        { token }
    )
}

export function createAppointment(token: string, fields: BookAppointmentFields) {
    return request<Appointment>("POST", "/appointments", {
        token,
        body: {
            doctorId: Number(fields.doctorId),
            scheduledAt: fields.scheduledAt,
        },
    })
}