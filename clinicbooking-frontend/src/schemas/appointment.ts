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