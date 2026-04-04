import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useGoogleLogin } from '@react-oauth/google';
import { createUser, createUserSSO } from '../services/api';

const Register = () => {
    const navigate = useNavigate();
    const [isGoogleLoading, setIsGoogleLoading] = useState(false);
    const [isFormLoading, setIsFormLoading] = useState(false);
    const [formData, setFormData] = useState({ name: '', email: '', password: '', confirmPassword: '' });
    const [formError, setFormError] = useState('');

    const handleInputChange = (e) => {
        const { id, value } = e.target;
        setFormData((prev) => ({ ...prev, [id]: value }));
    };

    const handleGoogleSignup = useGoogleLogin({
        onSuccess: async (tokenResponse) => {
            try {
                const createRes = await createUserSSO({
                    provider_name: 'google',
                    provider_access_token: tokenResponse.access_token,
                });

                if (createRes.status === 200 || createRes.status === 201) {
                    console.log('[Register] SSO user created successfully:', createRes.data);
                    navigate('/login');
                } else {
                    console.error('[Register] Failed to create SSO user:', createRes.status, createRes.data);
                }
            } catch (err) {
                console.error('[Google OAuth] Failed to create SSO user:', err);
            } finally {
                setIsGoogleLoading(false);
            }
        },
        onError: (error) => {
            console.error('[Google OAuth] Login error:', error);
            setIsGoogleLoading(false);
        },
        onNonOAuthError: (error) => {
            console.warn('[Google OAuth] Non-OAuth error (popup closed?):', error);
            setIsGoogleLoading(false);
        },
    });

    const handleGoogleButtonClick = () => {
        setIsGoogleLoading(true);
        handleGoogleSignup();
    };

    const handleFormSubmit = async () => {
        setFormError('');

        const { name, email, password, confirmPassword } = formData;

        if (!name || !email || !password || !confirmPassword) {
            setFormError('Please fill in all fields.');
            return;
        }

        if (password !== confirmPassword) {
            setFormError('Passwords do not match.');
            return;
        }

        try {
            setIsFormLoading(true);
            const createRes = await createUser({ name, email, password });

            if (createRes.status === 200 || createRes.status === 201) {
                console.log('[Register] User created successfully:', createRes.data);
                navigate('/login');
            } else {
                console.error('[Register] Failed to create user:', createRes.status, createRes.data);
                setFormError('Failed to create account. Please try again.');
            }
        } catch (err) {
            console.error('[Register] Error creating user:', err);
            setFormError('An unexpected error occurred. Please try again.');
        } finally {
            setIsFormLoading(false);
        }
    };

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
                        <button
                            id="btn-google-signup"
                            type="button"
                            disabled={isGoogleLoading}
                            onClick={handleGoogleButtonClick}
                            className="w-full flex items-center justify-center gap-3 bg-surface-container-low hover:bg-surface-container transition-all duration-200 py-4 px-6 rounded-xl group mb-8 disabled:opacity-60 disabled:cursor-not-allowed"
                        >
                            {isGoogleLoading ? (
                                <svg className="animate-spin h-5 w-5 text-on-surface-variant" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z" />
                                </svg>
                            ) : (
                                <svg aria-hidden="true" className="h-5 w-5" viewBox="0 0 24 24">
                                    <path d="M12.0003 20.45C16.6491 20.45 20.5505 17.2882 22.0003 13.0909H12.0003V10.9091H24.2307C24.4173 11.7582 24.5458 12.78 24.5458 14C24.5458 20.6273 19.1639 26 12.0003 26C5.37302 26 0.000289917 20.6273 0.000289917 14C0.000289917 7.37273 5.37302 2 12.0003 2V6.36364C7.78211 6.36364 4.36393 9.78182 4.36393 14C4.36393 18.2182 7.78211 21.6364 12.0003 21.6364V20.45Z" fill="#EA4335" transform="scale(0.8) translate(3,3)"></path>
                                    <path d="M24.2307 10.9091H12.0003V13.0909H22.0003C21.6493 14.2886 21.0526 15.3888 20.2679 16.3273L23.2384 19.2977C23.9457 18.5905 24.3821 17.6545 24.4173 11.7582L24.2307 10.9091Z" fill="#FBBC05" transform="scale(0.8) translate(3,3)"></path>
                                    <path d="M4.36393 14C4.36393 11.9695 5.12211 10.1373 6.34938 8.72727L3.37893 5.75682C1.29484 8.01818 0.000289917 10.8841 0.000289917 14H4.36393Z" fill="#34A853" transform="scale(0.8) translate(3,3)"></path>
                                    <path d="M12.0003 6.36364C13.8839 6.36364 15.6021 7.02955 16.9458 8.12727L20.0112 5.06182C17.9003 3.09091 15.093 2 12.0003 2V6.36364Z" fill="#4285F4" transform="scale(0.8) translate(3,3)"></path>
                                </svg>
                            )}
                            <span className="font-semibold text-on-surface-variant font-body">
                                {isGoogleLoading ? 'Redirecting...' : 'Sign up with Google'}
                            </span>
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
                                <input className="w-full bg-surface-container-low border-0 focus:ring-2 focus:ring-tertiary/40 rounded-xl py-4 px-5 text-on-surface placeholder:text-outline-variant transition-all font-body" id="name" placeholder="Alex Thompson" type="text" value={formData.name} onChange={handleInputChange} />
                            </div>
                            <div className="space-y-1.5">
                                <label className="block text-xs font-bold text-outline uppercase tracking-wider ml-1" htmlFor="email">Email Address</label>
                                <input className="w-full bg-surface-container-low border-0 focus:ring-2 focus:ring-tertiary/40 rounded-xl py-4 px-5 text-on-surface placeholder:text-outline-variant transition-all font-body" id="email" placeholder="alex@example.com" type="email" value={formData.email} onChange={handleInputChange} />
                            </div>
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                <div className="space-y-1.5">
                                    <label className="block text-xs font-bold text-outline uppercase tracking-wider ml-1" htmlFor="password">Password</label>
                                    <input className="w-full bg-surface-container-low border-0 focus:ring-2 focus:ring-tertiary/40 rounded-xl py-4 px-5 text-on-surface placeholder:text-outline-variant transition-all font-body" id="password" placeholder="••••••••" type="password" value={formData.password} onChange={handleInputChange} />
                                </div>
                                <div className="space-y-1.5">
                                    <label className="block text-xs font-bold text-outline uppercase tracking-wider ml-1" htmlFor="confirm_password">Confirm</label>
                                    <input className="w-full bg-surface-container-low border-0 focus:ring-2 focus:ring-tertiary/40 rounded-xl py-4 px-5 text-on-surface placeholder:text-outline-variant transition-all font-body" id="confirmPassword" placeholder="••••••••" type="password" value={formData.confirmPassword} onChange={handleInputChange} />
                                </div>
                            </div>

                            {/* CTA Section */}
                            {formError && (
                                <p className="text-red-500 text-sm font-body text-center">{formError}</p>
                            )}
                            <div className="pt-4">
                                <button
                                    className="w-full bg-primary hover:bg-primary-dim text-on-primary font-bold py-4 rounded-xl transition-all duration-300 shadow-lg shadow-primary/20 active:scale-[0.98] font-headline text-lg tracking-tight disabled:opacity-60 disabled:cursor-not-allowed"
                                    type="button"
                                    disabled={isFormLoading}
                                    onClick={handleFormSubmit}
                                >
                                    {isFormLoading ? 'Creating...' : 'Create Account'}
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
