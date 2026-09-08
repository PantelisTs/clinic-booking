import { z } from "zod"

export const loginSchema = z.object({
    username: z.string().min(1, { error: "Username is required" }),
    password: z.string().min(1, { error: "Password is required" }),
    keepLoggedIn: z.boolean(),
})

export type LoginFields = z.infer<typeof loginSchema>

export type LoginResponse = {
    token: string
}


const PASSWORD_REGEX = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,}$/

const passwordField = z
    .string()
    .min(1, { error: "Password is required" })
    .regex(PASSWORD_REGEX, {
        error: "Password must be 8+ characters with upper/lowercase, a number and a symbol",
    })

const baseSignupFields = {
    username: z
        .string()
        .min(2, { error: "Username must be between 2 and 50 characters" })
        .max(50, { error: "Username must be between 2 and 50 characters" }),
    email: z.string().min(1, { error: "Email is required" }).email({ error: "Invalid email address" }),
    password: passwordField,
    firstName: z
        .string()
        .min(2, { error: "First name must be between 2 and 50 characters" })
        .max(50, { error: "First name must be between 2 and 50 characters" }),
    lastName: z
        .string()
        .min(2, { error: "Last name must be between 2 and 50 characters" })
        .max(50, { error: "Last name must be between 2 and 50 characters" }),
}

export const doctorSignupSchema = z.object({
    ...baseSignupFields,
    specialty: z
        .string()
        .min(2, { error: "Specialty must be between 2 and 100 characters" })
        .max(100, { error: "Specialty must be between 2 and 100 characters" }),
})

export type DoctorSignupFields = z.infer<typeof doctorSignupSchema>

export const patientSignupSchema = z.object(baseSignupFields)

export type PatientSignupFields = z.infer<typeof patientSignupSchema>

export type SignupResponse = {
    id: number
    username: string
    email: string
    firstName: string
    lastName: string
    userRole: string
}