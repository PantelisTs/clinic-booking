import { z } from "zod"

export const loginSchema = z.object({
    username: z.string().min(1, { error: "Το username είναι υποχρεωτικό" }),
    password: z.string().min(1, { error: "Το password είναι υποχρεωτικό" }),
    keepLoggedIn: z.boolean(),
})

export type LoginFields = z.infer<typeof loginSchema>

export type LoginResponse = {
    token: string
}