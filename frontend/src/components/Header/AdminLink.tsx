"use client"

import { useSession } from "next-auth/react"
import Link from "next/link"

export default function AdminLink() {

    const { data: session } = useSession()

    return session?.user.isAdmin && (
        <Link href="/admin" className="font-semibold text-center text-base text-[#767676] hover:underline underline-offset-[4px] 
        decoration-[2px] hover:text-[#262626] border-r-[2px] px-7 border-r-gray-300 last:border-r-0 hidden md:inline">
            Admin
        </Link>
    )
}