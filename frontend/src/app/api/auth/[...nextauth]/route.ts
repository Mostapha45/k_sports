import GoogleProvider from 'next-auth/providers/google'
import NextAuth from "next-auth/next"
import { NextAuthOptions } from 'next-auth'
import { PrismaClient } from '@prisma/client'

const prisma = new PrismaClient()

const options: NextAuthOptions = {
    providers: [
        GoogleProvider({
            clientId: process.env.GOOGLE_OAUTH_CLIENT_ID ?? (() => { throw new Error("GOOGLE_OAUTH_CLIENT_ID is required") })(),
            clientSecret: process.env.GOOGLE_OAUTH_CLIENT_SECRET ?? (() => { throw new Error("GOOGLE_OAUTH_CLIENT_SECRET is required") })()
        })
    ],
    callbacks: {
        async jwt({ token }) {
            if (token.email) {
                const admin = await prisma.admins.findUnique({
                    where: {
                        Email: token.email
                    }
                });
                token.isAdmin = !!admin;
            }
            return token;
        },
        async session({ session, token }) {

            return {
                ...session,
                user: {
                    ...session.user,
                    isAdmin: token.isAdmin as boolean
                }
            }
        },
    },
    jwt: {
        maxAge: 60 * 60 * 24 * 15
    },
    secret: process.env.NEXTAUTH_SECRET,
}

const handler = NextAuth(options)

export { handler as GET, handler as POST }