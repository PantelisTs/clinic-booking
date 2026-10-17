import { request } from "@/api/http.ts"
import type { Doctor } from "@/schemas/doctor.ts"
import type { PaginatedResult } from "@/schemas/appointment.ts"

export function getDoctors(token: string) {
    return request<PaginatedResult<Doctor>>("GET", "/doctors?pageNumber=1&pageSize=100", { token })
}