import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useGoogleLogin } from '@react-oauth/google';
import { loginUser, loginUserSSO } from '../services/api';

export default function Login() {
    const navigate = useNavigate();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const [errors, setErrors] = useState([]);
    const [loading, setLoading] = useState(false);
    const [ssoLoading, setSsoLoading] = useState(false);

    // ─── helpers ──────────────────────────────────────────────────────────────

    /**
     * Persiste o token JWT no localStorage e redireciona para o dashboard.
     * @param {string} token
     */
    const handleAuthSuccess = (token) => {
        localStorage.setItem('auth_token', token);
        navigate('/dashboard');
    };

    /**
     * Extrai e exibe as mensagens de erro vindas da API.
     * @param {import('axios').AxiosResponse | null} response
     * @param {unknown} err
     */
    const handleApiError = (response, err) => {
        if (response?.data?.error && Array.isArray(response.data.messages)) {
            setErrors(response.data.messages);
        } else if (err?.response?.data?.messages) {
            setErrors(err.response.data.messages);
        } else {
            setErrors(['Ocorreu um erro inesperado. Tente novamente.']);
        }
    };

    // ─── email / senha ────────────────────────────────────────────────────────

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrors([]);
        setLoading(true);

        try {
            const response = await loginUser({ email, password });
            const body = response.data;

            if (body.success && body.data?.token) {
                handleAuthSuccess(body.data.token);
            } else {
                handleApiError(response, null);
            }
        } catch (err) {
            handleApiError(null, err);
        } finally {
            setLoading(false);
        }
    };

    // ─── Google SSO ───────────────────────────────────────────────────────────

    const handleGoogleLogin = useGoogleLogin({
        onSuccess: async (tokenResponse) => {
            setSsoLoading(true);
            setErrors([]);

            try {
                const response = await loginUserSSO({
                    provider_name: 'google',
                    provider_access_token: tokenResponse.access_token,
                });
                const body = response.data;

                if (body.success && body.data?.token) {
                    handleAuthSuccess(body.data.token);
                } else {
                    handleApiError(response, null);
                }
            } catch (err) {
                handleApiError(null, err);
            } finally {
                setSsoLoading(false);
            }
        },
        onError: () => {
            setErrors(['Falha ao autenticar com o Google. Tente novamente.']);
        },
    });

    // ─── render ───────────────────────────────────────────────────────────────

    const isSubmitting = loading || ssoLoading;

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
                        <p className="text-slate-500 dark:text-slate-400 text-sm mt-1">Focus &amp; Clarity for your finances</p>
                    </div>

                    {/* Error messages */}
                    {errors.length > 0 && (
                        <div className="mb-5 flex flex-col gap-1.5" role="alert" id="login-errors">
                            {errors.map((msg, idx) => (
                                <span
                                    key={idx}
                                    className="flex items-center gap-2 rounded-lg bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 text-red-600 dark:text-red-400 text-sm px-3 py-2"
                                >
                                    <span className="material-symbols-outlined text-[18px] shrink-0">error</span>
                                    {msg}
                                </span>
                            ))}
                        </div>
                    )}

                    {/* Login Form */}
                    <form onSubmit={handleSubmit} className="flex flex-col gap-5">
                        {/* Email Field */}
                        <div className="flex flex-col gap-1.5">
                            <label className="text-slate-900 dark:text-slate-200 text-sm font-medium leading-normal" htmlFor="email">Email</label>
                            <div className="relative flex items-center">
                                <div className="absolute left-4 text-slate-500 dark:text-slate-400 flex items-center justify-center pointer-events-none">
                                    <span className="material-symbols-outlined text-[20px]">mail</span>
                                </div>
                                <input
                                    id="email"
                                    className="form-input flex w-full rounded-lg border border-slate-300 dark:border-slate-600 bg-white dark:bg-slate-800 h-12 pl-11 pr-4 text-slate-900 dark:text-white placeholder:text-slate-400 focus:border-primary focus:ring-1 focus:ring-primary transition-colors text-sm font-normal"
                                    placeholder="user@example.com"
                                    type="email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                    disabled={isSubmitting}
                                />
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
                                <input
                                    id="password"
                                    className="form-input flex w-full rounded-lg border border-slate-300 dark:border-slate-600 bg-white dark:bg-slate-800 h-12 pl-11 pr-10 text-slate-900 dark:text-white placeholder:text-slate-400 focus:border-primary focus:ring-1 focus:ring-primary transition-colors text-sm font-normal"
                                    placeholder="••••••••"
                                    type={showPassword ? 'text' : 'password'}
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                    disabled={isSubmitting}
                                />
                                <button
                                    className="absolute right-3 text-slate-400 hover:text-slate-600 dark:hover:text-slate-200 flex items-center justify-center p-1 rounded-md transition-colors"
                                    type="button"
                                    onClick={() => setShowPassword((v) => !v)}
                                    aria-label={showPassword ? 'Ocultar senha' : 'Mostrar senha'}
                                >
                                    <span className="material-symbols-outlined text-[20px]">
                                        {showPassword ? 'visibility_off' : 'visibility'}
                                    </span>
                                </button>
                            </div>
                        </div>

                        {/* Forgot Password Link */}
                        <div className="flex justify-end">
                            <Link to="/forgot-password" className="text-primary text-sm font-medium hover:underline decoration-primary/50 underline-offset-4">
                                Forgot password?
                            </Link>
                        </div>

                        {/* Primary Action Button */}
                        <button
                            id="btn-login"
                            type="submit"
                            disabled={isSubmitting}
                            className="flex w-full cursor-pointer items-center justify-center gap-2 rounded-lg h-12 bg-secondary hover:bg-slate-900 dark:bg-primary dark:hover:bg-blue-600 text-white text-base font-bold transition-colors shadow-sm disabled:opacity-60 disabled:cursor-not-allowed"
                        >
                            {loading && (
                                <span className="material-symbols-outlined animate-spin text-[20px]">progress_activity</span>
                            )}
                            Enter
                        </button>

                        {/* Divider */}
                        <div className="relative flex py-2 items-center">
                            <div className="flex-grow border-t border-slate-300 dark:border-slate-700"></div>
                            <span className="flex-shrink mx-4 text-slate-400 dark:text-slate-500 text-xs font-medium uppercase tracking-wider">Or</span>
                            <div className="flex-grow border-t border-slate-300 dark:border-slate-700"></div>
                        </div>

                        {/* SSO Button */}
                        <button
                            id="btn-login-google"
                            className="flex w-full cursor-pointer items-center justify-center gap-3 rounded-lg h-12 bg-white dark:bg-slate-800 border border-slate-300 dark:border-slate-600 text-slate-700 dark:text-slate-200 text-sm font-bold hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors disabled:opacity-60 disabled:cursor-not-allowed"
                            type="button"
                            disabled={isSubmitting}
                            onClick={() => handleGoogleLogin()}
                        >
                            {ssoLoading ? (
                                <span className="material-symbols-outlined animate-spin text-[20px]">progress_activity</span>
                            ) : (
                                <svg aria-hidden="true" className="h-5 w-5" viewBox="0 0 24 24">
                                    <path d="M12.0003 20.45C16.6491 20.45 20.5505 17.2882 22.0003 13.0909H12.0003V10.9091H24.2307C24.4173 11.7582 24.5458 12.78 24.5458 14C24.5458 20.6273 19.1639 26 12.0003 26C5.37302 26 0.000289917 20.6273 0.000289917 14C0.000289917 7.37273 5.37302 2 12.0003 2V6.36364C7.78211 6.36364 4.36393 9.78182 4.36393 14C4.36393 18.2182 7.78211 21.6364 12.0003 21.6364V20.45Z" fill="#EA4335" transform="scale(0.8) translate(3,3)"></path>
                                    <path d="M24.2307 10.9091H12.0003V13.0909H22.0003C21.6493 14.2886 21.0526 15.3888 20.2679 16.3273L23.2384 19.2977C23.9457 18.5905 24.3821 17.6545 24.4173 11.7582L24.2307 10.9091Z" fill="#FBBC05" transform="scale(0.8) translate(3,3)"></path>
                                    <path d="M4.36393 14C4.36393 11.9695 5.12211 10.1373 6.34938 8.72727L3.37893 5.75682C1.29484 8.01818 0.000289917 10.8841 0.000289917 14H4.36393Z" fill="#34A853" transform="scale(0.8) translate(3,3)"></path>
                                    <path d="M12.0003 6.36364C13.8839 6.36364 15.6021 7.02955 16.9458 8.12727L20.0112 5.06182C17.9003 3.09091 15.093 2 12.0003 2V6.36364Z" fill="#4285F4" transform="scale(0.8) translate(3,3)"></path>
                                </svg>
                            )}
                            Continue with Google
                        </button>
                    </form>

                    {/* Create Account Link */}
                    <div className="mt-8 text-center">
                        <p className="text-slate-500 dark:text-slate-400 text-sm">
                            Don't have an account?
                            <Link className="text-primary font-bold hover:text-primary/80 transition-colors ml-1" to="/register">Create an account</Link>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}
