import { Button } from "@/components/ui/button.tsx"

type PaginationProps = {
    pageNumber: number
    totalPages: number
    onPageChange: (page: number) => void
}

export default function Pagination({ pageNumber, totalPages, onPageChange }: PaginationProps) {
    if (totalPages <= 1) return null

    return (
        <div className="mt-6 flex items-center justify-center gap-4">
            <Button
                variant="outline"
                size="sm"
                disabled={pageNumber <= 1}
                onClick={() => onPageChange(pageNumber - 1)}
            >
                Previous
            </Button>
            <span className="text-sm text-muted-foreground">
                Page {pageNumber} of {totalPages}
            </span>
            <Button
                variant="outline"
                size="sm"
                disabled={pageNumber >= totalPages}
                onClick={() => onPageChange(pageNumber + 1)}
            >
                Next
            </Button>
        </div>
    )
}