import type {Appointment, AppointmentStatus} from "@/schemas/appointment.ts"
import {Card, CardContent} from "@/components/ui/card.tsx"
import {Button} from "@/components/ui/button.tsx"

type AppointmentCardProps = {
    appointment: Appointment
    onCancel?: (appointment: Appointment) => void
}

const statusStyles: Record<AppointmentStatus, string> = {
    Pending: "bg-yellow-100 text-yellow-800",
    Confirmed: "bg-green-100 text-green-800",
    Cancelled: "bg-red-100 text-red-800",
    Completed: "bg-gray-200 text-gray-800",
}

export default function AppointmentCard({appointment, onCancel}: AppointmentCardProps) {
    const formattedDate = new Date(appointment.scheduledAt).toLocaleString("en-GB", {
        dateStyle: "medium",
        timeStyle: "short",
    })

    const canCancel =
        onCancel !== undefined &&
        (appointment.status === "Pending" || appointment.status === "Confirmed")

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
                <div className="flex flex-col items-end gap-2">
                    <span
                        className={`rounded-full px-3 py-1 text-xs font-medium ${statusStyles[appointment.status]}`}
                    >
                        {appointment.status}
                    </span>
                    {canCancel && (
                        <Button variant="outline" size="sm" onClick={() => onCancel(appointment)}>
                            Cancel
                        </Button>
                    )}
                </div>
            </CardContent>
        </Card>
    )
}