'use client'

import { redirect } from 'next/navigation'
import { useSession } from 'next-auth/react'

export default function AdminRoot({children}: {children: React.ReactNode}) {

    // Protect admin route
    const { data: session, status } = useSession()

    if (status === "loading") return <div>Loading...</div>

    if (status === "unauthenticated") {
        redirect('/api/auth/signin')
    }

    return status === "authenticated" && session.user.isAdmin && children
}