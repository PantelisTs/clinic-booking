import { z } from "zod"

export type AppointmentStatus = "Pending" | "Confirmed" | "Cancelled" | "Completed"

export type Appointment = {
    id: number
    scheduledAt: string
    status: AppointmentStatus
    notes?: string
    patientId: number
    patientName: string
    doctorId: number
    doctorName: string
}

export type PaginatedResult<T> = {
    data: T[]
    totalRecords: number
    pageNumber: number
    pageSize: number
    totalPages: number
}

export const bookAppointmentSchema = z.object({
    doctorId: z.string().min(1, { error: "Please select a doctor" }),
    scheduledAt: z
        .string()
        .min(1, { error: "Please select a date and time" })
        .refine((value) => new Date(value) > new Date(), {
            error: "Date and time must be in the future",
        }),
})

export type BookAppointmentFields = z.infer<typeof bookAppointmentSchema>