import React from 'react';
import { Link } from 'react-router-dom';

const Register = () => {
    return (
        <div className="bg-background text-on-background min-h-screen flex flex-col relative overflow-hidden">
            {/* Auth Shell Navigation */}
            <nav className="fixed top-0 w-full z-50 bg-white/80 backdrop-blur-md">
                <div className="flex justify-between items-center px-6 py-6 max-w-7xl mx-auto">
                    <div className="text-2xl font-bold tracking-tighter text-slate-700 font-headline">
                        BudgetControl
                    </div>
                    <div>
                        <Link className="text-secondary font-medium hover:text-primary transition-colors font-body text-sm" to="/support">
                            Support
                        </Link>
                    </div>
                </div>
            </nav>

            <main className="flex-grow flex items-center justify-center px-4 pt-24 pb-12 z-10">
                <div className="w-full max-w-md">
                    {/* Header Section */}
                    <div className="text-center mb-10">
                        <h1 className="text-4xl font-extrabold tracking-tight text-on-surface mb-3 font-headline">
                            Start your journey
                        </h1>
                        <p className="text-secondary font-body leading-relaxed">
                            The Silent Navigator for your family's financial future.
                        </p>
                    </div>

                    {/* Main Auth Card */}
                    <div className="bg-surface-container-lowest rounded-[2rem] p-8 shadow-[0px_24px_48px_rgba(45,52,53,0.06)]">
                        {/* SSO Integration */}
                        <button className="w-full flex items-center justify-center gap-3 bg-surface-container-low hover:bg-surface-container transition-all duration-200 py-4 px-6 rounded-xl group mb-8">
                            <img alt="Google Logo" className="w-5 h-5" src="https://lh3.googleusercontent.com/aida-public/AB6AXuDlti0ddZiOfybnF4IBGn3OzcKPQqEyU2FKzkDL9AbcOvSWKFvgIakxPLEwbj9HqLUwrK7ptD11_j8e2ioop2eKP6xH9wb2QClw5AFqLiw9r-RH8onVKQd6vsudO0ReRcNqGNC-289Sxo1ug_YGqouppYHb5MpcRFG6PPS6sfMOIb-ptjeHWhv8CqFRmtpMWpspZHXrgV186Ddf7R0LzTMkiRa2bFXzoqGfIPL4fOIu-K-uX1HxKu153quPW_rVTs_jXLaiDVRtMJI" />
                            <span className="font-semibold text-on-surface-variant font-body">Sign up with Google</span>
                        </button>
                        <div className="relative flex py-4 items-center mb-6">
                            <div className="flex-grow border-t border-outline-variant/20"></div>
                            <span className="flex-shrink mx-4 text-outline-variant text-xs font-bold tracking-widest uppercase">Or use email</span>
                            <div className="flex-grow border-t border-outline-variant/20"></div>
                        </div>

                        {/* Registration Form */}
                        <form className="space-y-5">
                            <div className="space-y-1.5">
                                <label className="block text-xs font-bold text-outline uppercase tracking-wider ml-1" htmlFor="name">Full Name</label>
                                <input className="w-full bg-surface-container-low border-0 focus:ring-2 focus:ring-tertiary/40 rounded-xl py-4 px-5 text-on-surface placeholder:text-outline-variant transition-all font-body" id="name" placeholder="Alex Thompson" type="text" />
                            </div>
                            <div className="space-y-1.5">
                                <label className="block text-xs font-bold text-outline uppercase tracking-wider ml-1" htmlFor="email">Email Address</label>
                                <input className="w-full bg-surface-container-low border-0 focus:ring-2 focus:ring-tertiary/40 rounded-xl py-4 px-5 text-on-surface placeholder:text-outline-variant transition-all font-body" id="email" placeholder="alex@example.com" type="email" />
                            </div>
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                <div className="space-y-1.5">
                                    <label className="block text-xs font-bold text-outline uppercase tracking-wider ml-1" htmlFor="password">Password</label>
                                    <input className="w-full bg-surface-container-low border-0 focus:ring-2 focus:ring-tertiary/40 rounded-xl py-4 px-5 text-on-surface placeholder:text-outline-variant transition-all font-body" id="password" placeholder="••••••••" type="password" />
                                </div>
                                <div className="space-y-1.5">
                                    <label className="block text-xs font-bold text-outline uppercase tracking-wider ml-1" htmlFor="confirm_password">Confirm</label>
                                    <input className="w-full bg-surface-container-low border-0 focus:ring-2 focus:ring-tertiary/40 rounded-xl py-4 px-5 text-on-surface placeholder:text-outline-variant transition-all font-body" id="confirm_password" placeholder="••••••••" type="password" />
                                </div>
                            </div>

                            {/* CTA Section */}
                            <div className="pt-4">
                                <button className="w-full bg-primary hover:bg-primary-dim text-on-primary font-bold py-4 rounded-xl transition-all duration-300 shadow-lg shadow-primary/20 active:scale-[0.98] font-headline text-lg tracking-tight" type="button">
                                    Create Account
                                </button>
                            </div>
                        </form>

                        {/* Redirect */}
                        <div className="mt-8 text-center">
                            <p className="text-secondary font-body text-sm">
                                Already have an account? 
                                <Link className="text-tertiary font-bold hover:underline ml-1" to="/login">Login</Link>
                            </p>
                        </div>
                    </div>

                    {/* Trust Footer */}
                    <div className="mt-12 text-center">
                        <div className="flex items-center justify-center gap-6 opacity-40 grayscale hover:grayscale-0 transition-all duration-500">
                            <span className="text-[10px] font-black tracking-[0.2em] uppercase text-outline">Trusted by modern families</span>
                        </div>
                        <div className="mt-8 flex justify-center gap-8 text-outline-variant text-xs font-medium">
                            <Link className="hover:text-primary" to="/privacy">Privacy Policy</Link>
                            <Link className="hover:text-primary" to="/terms">Terms of Service</Link>
                        </div>
                    </div>
                </div>
            </main>

            {/* Decorative Elements (Asymmetric layout helper) */}
            <div className="fixed -bottom-24 -left-24 w-96 h-96 bg-primary-container/20 rounded-full blur-[100px] pointer-events-none -z-10"></div>
            <div className="fixed top-24 -right-24 w-64 h-64 bg-tertiary-container/10 rounded-full blur-[80px] pointer-events-none -z-10"></div>
        </div>
    );
};

export default Register;
