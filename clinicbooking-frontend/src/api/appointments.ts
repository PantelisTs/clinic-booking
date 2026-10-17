import { request } from "@/api/http.ts"
import type {Appointment, AppointmentStatus, BookAppointmentFields, PaginatedResult} from "@/schemas/appointment.ts"

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

export function cancelAppointment(token: string, id: number) {
    return request<Appointment>("PATCH", `/appointments/${id}/cancel`, { token })
}

export function updateAppointment(
    token: string,
    id: number,
    changes: { notes?: string; status?: AppointmentStatus }
) {
    return request<Appointment>("PUT", `/appointments/${id}`, { token, body: changes })
}