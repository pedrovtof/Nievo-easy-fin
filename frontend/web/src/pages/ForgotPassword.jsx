import React from 'react';
import { Link } from 'react-router-dom';

export default function ForgotPassword() {
    return (
        <div className="bg-background-light dark:bg-background-dark text-[#131415] dark:text-white font-display min-h-screen">
            <div className="relative flex h-screen w-full flex-col overflow-x-hidden">
                {/* Top Navigation (Simplified for Recovery Screen) */}
                <header className="flex items-center justify-between whitespace-nowrap border-b border-solid border-b-[#f2f2f3] dark:border-b-[#2d3135] px-10 py-3 bg-white dark:bg-surface-dark shrink-0">
                    <div className="flex items-center gap-4 text-[#131415] dark:text-white">
                        <div className="size-6 text-primary dark:text-white">
                            <span className="material-symbols-outlined text-primary text-2xl">account_balance_wallet</span>
                        </div>
                        <h2 className="text-lg font-bold leading-tight tracking-[-0.015em]">BudgetManager</h2>
                    </div>
                </header>

                {/* Main Content Area */}
                <div className="flex flex-1 items-center justify-center p-4 sm:p-6">
                    <div className="layout-content-container flex flex-col w-full max-w-[480px]">
                        {/* Central Card */}
                        <div className="bg-white dark:bg-surface-dark rounded-xl shadow-lg border border-[#e5e7eb] dark:border-[#2d3135] p-6 sm:p-10 flex flex-col items-center text-center">
                            {/* Icon */}
                            <div className="mb-6 flex h-16 w-16 items-center justify-center rounded-full bg-primary/10 dark:bg-white/10">
                                <span className="material-symbols-outlined text-4xl text-primary dark:text-white">
                                    lock_reset
                                </span>
                            </div>
                            {/* Title & Instruction */}
                            <h1 className="text-[#131415] dark:text-white text-2xl font-bold leading-tight tracking-[-0.015em] mb-2">
                                Reset Password
                            </h1>
                            <p className="text-[#6f747b] dark:text-[#9ca3af] text-base font-medium leading-normal mb-8 max-w-[360px]">
                                Enter the email associated with your account and we'll send you a link to reset your password.
                            </p>
                            {/* Form */}
                            <div className="w-full flex flex-col gap-6">
                                {/* Input Field */}
                                <div className="flex flex-col text-left">
                                    <label className="flex flex-col w-full">
                                        <span className="text-[#131415] dark:text-white text-sm font-semibold leading-normal pb-2">Email Address</span>
                                        <input className="form-input flex w-full resize-none overflow-hidden rounded-lg text-[#131415] dark:text-white focus:outline-0 focus:ring-2 focus:ring-primary/20 border border-[#dfe0e2] dark:border-[#3f444d] bg-white dark:bg-[#1a1d21] focus:border-primary h-12 placeholder:text-[#6f747b] px-4 text-base font-normal leading-normal transition-colors" placeholder="you@example.com" type="email" />
                                    </label>
                                </div>
                                {/* Primary CTA */}
                                <button className="flex w-full cursor-pointer items-center justify-center overflow-hidden rounded-lg h-12 bg-primary hover:bg-[#1e2530] transition-colors text-white text-base font-bold leading-normal tracking-[0.015em] shadow-sm">
                                    <span className="truncate">Send Reset Link</span>
                                </button>
                            </div>
                            {/* Footer Link */}
                            <div className="mt-8">
                                <Link to="/login" className="group flex items-center gap-2 text-[#6f747b] dark:text-[#9ca3af] hover:text-primary dark:hover:text-white transition-colors text-sm font-medium leading-normal">
                                    <span className="material-symbols-outlined text-lg transition-transform group-hover:-translate-x-1">arrow_back</span>
                                    <span>Return to Login</span>
                                </Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
