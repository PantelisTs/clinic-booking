import type {
    LoginFields,
    LoginResponse,
    DoctorSignupFields,
    PatientSignupFields,
    SignupResponse,
} from "@/schemas/auth.ts"
import { request } from "@/api/http.ts"

export function login(fields: LoginFields) {
    return request<LoginResponse>("POST", "/auth/login", { body: fields })
}

export function registerDoctor(fields: DoctorSignupFields) {
    return request<SignupResponse>("POST", "/auth/register/doctor", {
        body: { ...fields, roleId: 2 },
    })
}

export function registerPatient(fields: PatientSignupFields) {
    return request<SignupResponse>("POST", "/auth/register/patient", {
        body: { ...fields, roleId: 3 },
    })
}