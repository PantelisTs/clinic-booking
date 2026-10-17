import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { toast } from "sonner"
import { Button } from "@/components/ui/button.tsx"
import { Input } from "@/components/ui/input.tsx"
import { Label } from "@/components/ui/label.tsx"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card.tsx"
import { useAuth } from "@/context/AuthProvider.tsx"
import { createAppointment } from "@/api/appointments.ts"
import { UnauthorizedError } from "@/api/http.ts"
import {
    type Appointment,
    type BookAppointmentFields,
    bookAppointmentSchema,
} from "@/schemas/appointment.ts"
import type { Doctor } from "@/schemas/doctor.ts"

type BookAppointmentFormProps = {
    doctors: Doctor[]
    onBooked: (appointment: Appointment) => void
}

export default function BookAppointmentForm({ doctors, onBooked }: BookAppointmentFormProps) {
    const { token, logoutUser } = useAuth()

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
    } = useForm<BookAppointmentFields>({
        resolver: zodResolver(bookAppointmentSchema),
    })

    const onSubmit = async (data: BookAppointmentFields) => {
        if (!token) return
        try {
            const appointment = await createAppointment(token, data)
            toast.success("Appointment booked")
            onBooked(appointment)
            reset()
        } catch (error) {
            if (error instanceof UnauthorizedError) logoutUser()
            toast.error(error instanceof Error ? error.message : "Booking failed")
        }
    }

    return (
        <Card className="mb-6">
            <CardHeader>
                <CardTitle>Book a new appointment</CardTitle>
            </CardHeader>
            <CardContent>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div className="space-y-2">
                        <Label htmlFor="doctorId">Doctor</Label>
                        <select
                            id="doctorId"
                            className="h-9 w-full rounded-md border border-input bg-transparent px-3 text-sm shadow-xs"
                            {...register("doctorId")}
                        >
                            <option value="">Select a doctor</option>
                            {doctors.map((doctor) => (
                                <option key={doctor.id} value={doctor.id}>
                                    Dr. {doctor.firstName} {doctor.lastName} — {doctor.specialty}
                                </option>
                            ))}
                        </select>
                        {errors.doctorId && (
                            <p className="text-sm text-destructive">{errors.doctorId.message}</p>
                        )}
                    </div>
                    <div className="space-y-2">
                        <Label htmlFor="scheduledAt">Date and time</Label>
                        <Input id="scheduledAt" type="datetime-local" {...register("scheduledAt")} />
                        {errors.scheduledAt && (
                            <p className="text-sm text-destructive">{errors.scheduledAt.message}</p>
                        )}
                    </div>
                    <Button type="submit" disabled={isSubmitting}>
                        {isSubmitting ? "Booking..." : "Book appointment"}
                    </Button>
                </form>
            </CardContent>
        </Card>
    )
}