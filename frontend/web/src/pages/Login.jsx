import React from 'react';
import { Link } from 'react-router-dom';

export default function Login() {
    return (
        <div className="font-display bg-background-light dark:bg-background-dark min-h-screen flex items-center justify-center p-4">
            {/* Main Layout Container */}
            <div className="w-full max-w-[440px] flex flex-col">
                {/* Login Card */}
                <div className="bg-card-light dark:bg-card-dark rounded-xl p-8 shadow-sm border border-slate-100 dark:border-slate-800 w-full">
                    {/* Header: Logo & Title */}
                    <div className="flex flex-col items-center justify-center mb-8">
                        <div className="w-12 h-12 bg-primary/10 rounded-lg flex items-center justify-center mb-3">
                            <span className="material-symbols-outlined text-primary text-[32px]">account_balance_wallet</span>
                        </div>
                        <h1 className="text-slate-900 dark:text-white text-2xl font-bold tracking-tight">BudgetControl</h1>
                        <p className="text-slate-500 dark:text-slate-400 text-sm mt-1">Focus & Clarity for your finances</p>
                    </div>
                    {/* Login Form */}
                    <form action="#" className="flex flex-col gap-5">
                        {/* Email Field */}
                        <div className="flex flex-col gap-1.5">
                            <label className="text-slate-900 dark:text-slate-200 text-sm font-medium leading-normal" htmlFor="email">Email</label>
                            <div className="relative flex items-center">
                                <div className="absolute left-4 text-slate-500 dark:text-slate-400 flex items-center justify-center pointer-events-none">
                                    <span className="material-symbols-outlined text-[20px]">mail</span>
                                </div>
                                <input className="form-input flex w-full rounded-lg border border-slate-300 dark:border-slate-600 bg-white dark:bg-slate-800 h-12 pl-11 pr-4 text-slate-900 dark:text-white placeholder:text-slate-400 focus:border-primary focus:ring-1 focus:ring-primary transition-colors text-sm font-normal" id="email" placeholder="user@example.com" type="email" />
                            </div>
                        </div>
                        {/* Password Field */}
                        <div className="flex flex-col gap-1.5">
                            <div className="flex justify-between items-center">
                                <label className="text-slate-900 dark:text-slate-200 text-sm font-medium leading-normal" htmlFor="password">Password</label>
                            </div>
                            <div className="relative flex items-center">
                                <div className="absolute left-4 text-slate-500 dark:text-slate-400 flex items-center justify-center pointer-events-none">
                                    <span className="material-symbols-outlined text-[20px]">lock</span>
                                </div>
                                <input className="form-input flex w-full rounded-lg border border-slate-300 dark:border-slate-600 bg-white dark:bg-slate-800 h-12 pl-11 pr-4 text-slate-900 dark:text-white placeholder:text-slate-400 focus:border-primary focus:ring-1 focus:ring-primary transition-colors text-sm font-normal" id="password" placeholder="••••••••" type="password" />
                                <button className="absolute right-3 text-slate-400 hover:text-slate-600 dark:hover:text-slate-200 flex items-center justify-center p-1 rounded-md transition-colors" type="button">
                                    <span className="material-symbols-outlined text-[20px]">visibility</span>
                                </button>
                            </div>
                        </div>
                        {/* Forgot Password Link */}
                        <div className="flex justify-end">
                            <Link to="/forgot-password" class="text-primary text-sm font-medium hover:underline decoration-primary/50 underline-offset-4">Forgot password?</Link>
                        </div>
                        {/* Primary Action Button */}
                        <Link to="/dashboard" className="flex w-full cursor-pointer items-center justify-center rounded-lg h-12 bg-secondary hover:bg-slate-900 dark:bg-primary dark:hover:bg-blue-600 text-white text-base font-bold transition-colors shadow-sm">
                            Enter
                        </Link>
                        {/* Divider */}
                        <div className="relative flex py-2 items-center">
                            <div className="flex-grow border-t border-slate-300 dark:border-slate-700"></div>
                            <span className="flex-shrink mx-4 text-slate-400 dark:text-slate-500 text-xs font-medium uppercase tracking-wider">Or</span>
                            <div className="flex-grow border-t border-slate-300 dark:border-slate-700"></div>
                        </div>
                        {/* SSO Button */}
                        <button className="flex w-full cursor-pointer items-center justify-center gap-3 rounded-lg h-12 bg-white dark:bg-slate-800 border border-slate-300 dark:border-slate-600 text-slate-700 dark:text-slate-200 text-sm font-bold hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors" type="button">
                            <svg aria-hidden="true" className="h-5 w-5" viewBox="0 0 24 24">
                                <path d="M12.0003 20.45C16.6491 20.45 20.5505 17.2882 22.0003 13.0909H12.0003V10.9091H24.2307C24.4173 11.7582 24.5458 12.78 24.5458 14C24.5458 20.6273 19.1639 26 12.0003 26C5.37302 26 0.000289917 20.6273 0.000289917 14C0.000289917 7.37273 5.37302 2 12.0003 2V6.36364C7.78211 6.36364 4.36393 9.78182 4.36393 14C4.36393 18.2182 7.78211 21.6364 12.0003 21.6364V20.45Z" fill="#EA4335" transform="scale(0.8) translate(3,3)"></path>
                                <path d="M24.2307 10.9091H12.0003V13.0909H22.0003C21.6493 14.2886 21.0526 15.3888 20.2679 16.3273L23.2384 19.2977C23.9457 18.5905 24.3821 17.6545 24.4173 11.7582L24.2307 10.9091Z" fill="#FBBC05" transform="scale(0.8) translate(3,3)"></path>
                                <path d="M4.36393 14C4.36393 11.9695 5.12211 10.1373 6.34938 8.72727L3.37893 5.75682C1.29484 8.01818 0.000289917 10.8841 0.000289917 14H4.36393Z" fill="#34A853" transform="scale(0.8) translate(3,3)"></path>
                                <path d="M12.0003 6.36364C13.8839 6.36364 15.6021 7.02955 16.9458 8.12727L20.0112 5.06182C17.9003 3.09091 15.093 2 12.0003 2V6.36364Z" fill="#4285F4" transform="scale(0.8) translate(3,3)"></path>
                            </svg>
                            Continue with Google
                        </button>
                    </form>
                    {/* Create Account Link */}
                    <div className="mt-8 text-center">
                        <p className="text-slate-500 dark:text-slate-400 text-sm">
                            Don't have an account?
                            <Link className="text-primary font-bold hover:text-primary/80 transition-colors ml-1" to="/cadastro">Create an account</Link>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}
