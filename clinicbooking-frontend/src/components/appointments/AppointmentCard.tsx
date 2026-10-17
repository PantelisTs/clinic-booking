import type { Appointment, AppointmentStatus } from "@/schemas/appointment.ts"
import { Card, CardContent } from "@/components/ui/card.tsx"

type AppointmentCardProps = {
    appointment: Appointment
}

const statusStyles: Record<AppointmentStatus, string> = {
    Pending: "bg-yellow-100 text-yellow-800",
    Confirmed: "bg-green-100 text-green-800",
    Cancelled: "bg-red-100 text-red-800",
    Completed: "bg-gray-200 text-gray-800",
}

export default function AppointmentCard({ appointment }: AppointmentCardProps) {
    const formattedDate = new Date(appointment.scheduledAt).toLocaleString("en-GB", {
        dateStyle: "medium",
        timeStyle: "short",
    })

    return (
        <Card>
            <CardContent className="flex items-center justify-between gap-4">
                <div>
                    <p className="font-medium">{formattedDate}</p>
                    <p className="text-sm text-muted-foreground">
                        Doctor: {appointment.doctorName}
                    </p>
                    <p className="text-sm text-muted-foreground">
                        Patient: {appointment.patientName}
                    </p>
                    {appointment.notes && (
                        <p className="text-sm mt-1">Notes: {appointment.notes}</p>
                    )}
                </div>
                <span
                    className={`rounded-full px-3 py-1 text-xs font-medium ${statusStyles[appointment.status]}`}
                >
                    {appointment.status}
                </span>
            </CardContent>
        </Card>
    )
}