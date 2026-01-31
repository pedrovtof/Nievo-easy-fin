import React from 'react';
import { Link } from 'react-router-dom';
import { useLanguage } from '../context/LanguageContext';
import Button from '../components/ui/Button';
import Input from '../components/ui/Input';
import Card from '../components/ui/Card';

export default function ForgotPassword() {
    const { t } = useLanguage();

    return (
        <div className="bg-background-light dark:bg-background-dark text-slate-900 dark:text-white font-display min-h-screen">
            <div className="relative flex h-screen w-full flex-col overflow-x-hidden">
                {/* Top Navigation (Simplified for Recovery Screen) */}
                <header className="flex items-center justify-between whitespace-nowrap border-b border-solid border-slate-200 dark:border-slate-800 px-10 py-3 bg-white dark:bg-card-dark shrink-0">
                    <div className="flex items-center gap-4 text-slate-900 dark:text-white">
                        <div className="size-6 text-primary dark:text-white">
                            <span className="material-symbols-outlined text-primary text-2xl">account_balance_wallet</span>
                        </div>
                        <h2 className="text-lg font-bold leading-tight tracking-[-0.015em]">Easy Fin</h2>
                    </div>
                </header>

                {/* Main Content Area */}
                <div className="flex flex-1 items-center justify-center p-4 sm:p-6">
                    <div className="layout-content-container flex flex-col w-full max-w-[480px]">
                        {/* Central Card */}
                        <Card className="flex flex-col items-center text-center">
                            {/* Icon */}
                            <div className="mb-6 flex h-16 w-16 items-center justify-center rounded-full bg-primary/10 dark:bg-white/10">
                                <span className="material-symbols-outlined text-4xl text-primary dark:text-white">
                                    lock_reset
                                </span>
                            </div>
                            {/* Title & Instruction */}
                            <h1 className="text-slate-900 dark:text-white text-2xl font-bold leading-tight tracking-[-0.015em] mb-2">
                                {t('forgot_password.title')}
                            </h1>
                            <p className="text-slate-500 dark:text-slate-400 text-base font-medium leading-normal mb-8 max-w-[360px]">
                                {t('forgot_password.instruction')}
                            </p>
                            {/* Form */}
                            <div className="w-full flex flex-col gap-6">
                                {/* Input Field */}
                                <div className="flex flex-col text-left">
                                    <Input
                                        label={t('forgot_password.email_label')}
                                        id="email"
                                        type="email"
                                        placeholder={t('forgot_password.email_placeholder')}
                                    />
                                </div>
                                {/* Primary CTA */}
                                <Button className="w-full">
                                    <span className="truncate">{t('forgot_password.submit_button')}</span>
                                </Button>
                            </div>
                            {/* Footer Link */}
                            <div className="mt-8">
                                <Link to="/login" className="group flex items-center gap-2 text-slate-500 dark:text-slate-400 hover:text-primary dark:hover:text-white transition-colors text-sm font-medium leading-normal">
                                    <span className="material-symbols-outlined text-lg transition-transform group-hover:-translate-x-1">arrow_back</span>
                                    <span>{t('forgot_password.back_to_login')}</span>
                                </Link>
                            </div>
                        </Card>
                    </div>
                </div>
            </div>
        </div>
    );
}
