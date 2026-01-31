import React, { useState } from 'react';
import { useLanguage } from '../context/LanguageContext';
import { useTheme } from '../context/ThemeContext';
import Button from '../components/ui/Button';
import Input from '../components/ui/Input';
import Card from '../components/ui/Card';

export default function Settings() {
    const { t, language, setLanguage } = useLanguage();
    const { theme, toggleTheme } = useTheme();

    const [profile, setProfile] = useState({
        fullName: 'Alex Morgan',
        email: 'alex.morgan@example.com',
        currency: 'USD'
    });

    const [toggles, setToggles] = useState({
        twoFactor: false,
        weeklyDigest: true,
        unusualActivity: true,
        budgetWarnings: false
    });

    const handleToggle = (key) => {
        setToggles(prev => ({ ...prev, [key]: !prev[key] }));
    };

    return (
        <div className="flex-1 overflow-y-auto bg-background-light dark:bg-background-dark relative">
            <div className="max-w-[1000px] mx-auto px-6 py-8 md:px-12 md:py-12">
                {/* Page Heading */}
                <div className="flex flex-col gap-2 mb-10">
                    <h1 className="text-3xl md:text-4xl font-black text-primary dark:text-white tracking-tight">{t('settings.title')}</h1>
                    <p className="text-gray-500 dark:text-gray-400 text-lg">{t('settings.subtitle')}</p>
                </div>

                {/* Sections Grid */}
                <div className="grid grid-cols-1 lg:grid-cols-12 gap-8">
                    {/* Left Column: Profile & Security */}
                    <div className="lg:col-span-7 flex flex-col gap-8">
                        {/* Profile Information */}
                        <Card>
                            <div className="flex items-center gap-3 mb-6 border-b border-gray-200 dark:border-gray-700 pb-4">
                                <span className="material-symbols-outlined text-primary dark:text-gray-300">person</span>
                                <h2 className="text-xl font-bold text-primary dark:text-white">{t('settings.profile_section_title')}</h2>
                            </div>
                            <div className="flex flex-col gap-5">
                                <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                                    <Input
                                        label={t('settings.full_name_label')}
                                        id="fullName"
                                        value={profile.fullName}
                                        onChange={(e) => setProfile({ ...profile, fullName: e.target.value })}
                                    />
                                    <Input
                                        label={t('settings.email_label')}
                                        id="email"
                                        type="email"
                                        value={profile.email}
                                        onChange={(e) => setProfile({ ...profile, email: e.target.value })}
                                    />
                                </div>
                                <label className="flex flex-col gap-2">
                                    <span className="text-sm font-semibold text-primary dark:text-gray-300">{t('settings.currency_label')}</span>
                                    <div className="relative">
                                        <select
                                            className="w-full h-12 px-4 appearance-none rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-primary dark:text-white focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all outline-none"
                                            value={profile.currency}
                                            onChange={(e) => setProfile({ ...profile, currency: e.target.value })}
                                        >
                                            <option value="USD">USD - United States Dollar</option>
                                            <option value="EUR">EUR - Euro</option>
                                            <option value="GBP">GBP - British Pound</option>
                                            <option value="CAD">CAD - Canadian Dollar</option>
                                        </select>
                                        <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-4 text-gray-500">
                                            <span className="material-symbols-outlined">expand_more</span>
                                        </div>
                                    </div>
                                    <p className="text-xs text-gray-500 mt-1">{t('settings.currency_help')}</p>
                                </label>
                            </div>
                        </Card>

                        {/* Security */}
                        <Card>
                            <div className="flex items-center gap-3 mb-6 border-b border-gray-200 dark:border-gray-700 pb-4">
                                <span className="material-symbols-outlined text-primary dark:text-gray-300">lock</span>
                                <h2 className="text-xl font-bold text-primary dark:text-white">{t('settings.security_section_title')}</h2>
                            </div>
                            <div className="flex flex-col gap-6">
                                <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                                    <div>
                                        <p className="text-sm font-bold text-primary dark:text-white">{t('settings.password_title')}</p>
                                        <p className="text-xs text-gray-500">{t('settings.last_changed')}</p>
                                    </div>
                                    <Button variant="secondary" className="px-4 py-2 h-auto text-sm font-semibold">
                                        {t('settings.change_password_button')}
                                    </Button>
                                </div>
                                <div className="h-px bg-gray-200 dark:bg-gray-700"></div>
                                <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                                    <div className="max-w-md">
                                        <p className="text-sm font-bold text-primary dark:text-white">{t('settings.2fa_title')}</p>
                                        <p className="text-xs text-gray-500 mt-1">{t('settings.2fa_description')}</p>
                                    </div>
                                    {/* Toggle */}
                                    <label className="flex items-center cursor-pointer relative" htmlFor="2fa-toggle">
                                        <input
                                            className="sr-only peer"
                                            id="2fa-toggle"
                                            type="checkbox"
                                            checked={toggles.twoFactor}
                                            onChange={() => handleToggle('twoFactor')}
                                        />
                                        <div className="w-11 h-6 bg-gray-300 peer-focus:outline-none peer-focus:ring-2 peer-focus:ring-primary/20 rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-primary"></div>
                                    </label>
                                </div>
                            </div>
                        </Card>

                        {/* Connected Accounts */}
                        <Card>
                            <div className="flex items-center justify-between mb-6 border-b border-gray-200 dark:border-gray-700 pb-4">
                                <div className="flex items-center gap-3">
                                    <span className="material-symbols-outlined text-primary dark:text-gray-300">account_balance</span>
                                    <h2 className="text-xl font-bold text-primary dark:text-white">{t('settings.connected_accounts_title')}</h2>
                                </div>
                                <button className="text-primary dark:text-white hover:bg-black/5 dark:hover:bg-white/10 p-2 rounded-full transition-colors">
                                    <span className="material-symbols-outlined">add</span>
                                </button>
                            </div>
                            <div className="flex flex-col gap-4">
                                {/* Connected Bank Item 1 */}
                                <div className="flex items-center justify-between p-4 bg-white dark:bg-gray-800 rounded-lg border border-gray-200 dark:border-gray-700 text-sm">
                                    <div className="flex items-center gap-4">
                                        <div className="w-10 h-10 rounded-full bg-blue-600 flex items-center justify-center text-white font-bold text-xs">
                                            CH
                                        </div>
                                        <div>
                                            <p className="font-bold text-primary dark:text-white">Chase Checking</p>
                                            <p className="text-xs text-gray-500">**** 4589 • Updated 2 mins ago</p>
                                        </div>
                                    </div>
                                    <button className="text-xs font-semibold text-red-600 hover:text-red-700 px-3 py-1.5 rounded bg-red-50 hover:bg-red-100 transition-colors">
                                        {t('settings.disconnect_button')}
                                    </button>
                                </div>
                                {/* Connected Bank Item 2 */}
                                <div className="flex items-center justify-between p-4 bg-white dark:bg-gray-800 rounded-lg border border-gray-200 dark:border-gray-700 text-sm">
                                    <div className="flex items-center gap-4">
                                        <div className="w-10 h-10 rounded-full bg-red-600 flex items-center justify-center text-white font-bold text-xs">
                                            WF
                                        </div>
                                        <div>
                                            <p className="font-bold text-primary dark:text-white">Wells Fargo Savings</p>
                                            <p className="text-xs text-gray-500">**** 9921 • Updated 1 hour ago</p>
                                        </div>
                                    </div>
                                    <button className="text-xs font-semibold text-red-600 hover:text-red-700 px-3 py-1.5 rounded bg-red-50 hover:bg-red-100 transition-colors">
                                        {t('settings.disconnect_button')}
                                    </button>
                                </div>
                                <button className="w-full py-3 mt-2 border-2 border-dashed border-gray-300 dark:border-gray-600 rounded-lg text-sm font-semibold text-gray-500 hover:text-primary dark:hover:text-white hover:border-primary dark:hover:border-gray-400 transition-colors flex items-center justify-center gap-2">
                                    <span className="material-symbols-outlined text-[18px]">add_circle</span>
                                    {t('settings.connect_new_button')}
                                </button>
                            </div>
                        </Card>
                    </div>

                    {/* Right Column: Preferences & Actions */}
                    <div className="lg:col-span-5 flex flex-col gap-8">
                        {/* Preferences */}
                        <Card className="h-fit">
                            <div className="flex items-center gap-3 mb-6 border-b border-gray-200 dark:border-gray-700 pb-4">
                                <span className="material-symbols-outlined text-primary dark:text-gray-300">tune</span>
                                <h2 className="text-xl font-bold text-primary dark:text-white">{t('settings.preferences_title')}</h2>
                            </div>
                            <div className="flex flex-col gap-6">
                                {/* Language Selection (New) */}
                                <div>
                                    <p className="text-sm font-bold text-primary dark:text-white mb-2">Language</p>
                                    <div className="relative">
                                        <select
                                            className="w-full h-10 px-3 appearance-none rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-primary dark:text-white focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all outline-none text-sm"
                                            value={language}
                                            onChange={(e) => setLanguage(e.target.value)}
                                        >
                                            <option value="en-us">English (US)</option>
                                            <option value="pt-br">Português (BR)</option>
                                        </select>
                                        <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-4 text-gray-500">
                                            <span className="material-symbols-outlined text-[18px]">expand_more</span>
                                        </div>
                                    </div>
                                </div>

                                <div className="h-px bg-gray-200 dark:bg-gray-700"></div>

                                {/* Dark Mode */}
                                <div className="flex items-center justify-between">
                                    <div className="flex items-center gap-3">
                                        <div className="p-2 bg-white dark:bg-gray-800 rounded-lg border border-gray-200 dark:border-gray-600">
                                            <span className="material-symbols-outlined text-primary dark:text-white text-[20px]">dark_mode</span>
                                        </div>
                                        <div>
                                            <p className="text-sm font-bold text-primary dark:text-white">{t('settings.dark_mode_title')}</p>
                                            <p className="text-xs text-gray-500">{t('settings.dark_mode_description')}</p>
                                        </div>
                                    </div>
                                    <label className="flex items-center cursor-pointer relative" htmlFor="dark-toggle">
                                        <input
                                            className="sr-only peer"
                                            id="dark-toggle"
                                            type="checkbox"
                                            checked={theme === 'dark'}
                                            onChange={toggleTheme}
                                        />
                                        <div className="w-11 h-6 bg-gray-300 peer-focus:outline-none peer-focus:ring-2 peer-focus:ring-primary/20 rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-primary"></div>
                                    </label>
                                </div>
                                <div className="h-px bg-gray-200 dark:bg-gray-700"></div>
                                {/* Notifications */}
                                <div>
                                    <p className="text-sm font-bold text-primary dark:text-white mb-4">{t('settings.notifications_title')}</p>
                                    <div className="flex flex-col gap-3">
                                        <label className="flex items-start gap-3 cursor-pointer group">
                                            <div className="relative flex items-center">
                                                <input
                                                    className="peer h-5 w-5 cursor-pointer appearance-none rounded border border-gray-300 checked:bg-primary checked:border-primary transition-all"
                                                    type="checkbox"
                                                    checked={toggles.weeklyDigest}
                                                    onChange={() => handleToggle('weeklyDigest')}
                                                />
                                                <span className="material-symbols-outlined absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 text-[16px] text-white opacity-0 peer-checked:opacity-100 pointer-events-none">check</span>
                                            </div>
                                            <div className="flex-1">
                                                <p className="text-sm font-medium text-primary dark:text-gray-300 group-hover:text-primary transition-colors">{t('settings.weekly_digest_title')}</p>
                                                <p className="text-xs text-gray-500">{t('settings.weekly_digest_desc')}</p>
                                            </div>
                                        </label>
                                        <label className="flex items-start gap-3 cursor-pointer group">
                                            <div className="relative flex items-center">
                                                <input
                                                    className="peer h-5 w-5 cursor-pointer appearance-none rounded border border-gray-300 checked:bg-primary checked:border-primary transition-all"
                                                    type="checkbox"
                                                    checked={toggles.unusualActivity}
                                                    onChange={() => handleToggle('unusualActivity')}
                                                />
                                                <span className="material-symbols-outlined absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 text-[16px] text-white opacity-0 peer-checked:opacity-100 pointer-events-none">check</span>
                                            </div>
                                            <div className="flex-1">
                                                <p className="text-sm font-medium text-primary dark:text-gray-300 group-hover:text-primary transition-colors">{t('settings.unusual_activity_title')}</p>
                                                <p className="text-xs text-gray-500">{t('settings.unusual_activity_desc')}</p>
                                            </div>
                                        </label>
                                        <label className="flex items-start gap-3 cursor-pointer group">
                                            <div className="relative flex items-center">
                                                <input
                                                    className="peer h-5 w-5 cursor-pointer appearance-none rounded border border-gray-300 checked:bg-primary checked:border-primary transition-all"
                                                    type="checkbox"
                                                    checked={toggles.budgetWarnings}
                                                    onChange={() => handleToggle('budgetWarnings')}
                                                />
                                                <span className="material-symbols-outlined absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 text-[16px] text-white opacity-0 peer-checked:opacity-100 pointer-events-none">check</span>
                                            </div>
                                            <div className="flex-1">
                                                <p className="text-sm font-medium text-primary dark:text-gray-300 group-hover:text-primary transition-colors">{t('settings.budget_warnings_title')}</p>
                                                <p className="text-xs text-gray-500">{t('settings.budget_warnings_desc')}</p>
                                            </div>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </Card>

                        {/* Action Buttons (Sticky/Floating effect via margin) */}
                        <div className="bg-white dark:bg-card-dark rounded-xl p-6 md:p-8 shadow-sm border border-gray-100 dark:border-gray-800">
                            <div className="flex flex-col gap-3">
                                <button className="w-full bg-primary hover:bg-primary/90 text-white font-bold py-3.5 px-6 rounded-lg transition-all shadow-md hover:shadow-lg flex items-center justify-center gap-2">
                                    <span className="material-symbols-outlined text-[20px]">save</span>
                                    {t('settings.save_changes_button')}
                                </button>
                                <button className="w-full bg-transparent hover:bg-gray-100 dark:hover:bg-gray-800 text-gray-500 dark:text-gray-400 font-bold py-3.5 px-6 rounded-lg transition-colors">
                                    {t('settings.cancel_button')}
                                </button>
                            </div>
                        </div>

                        {/* Help Card */}
                        <div className="bg-blue-50 dark:bg-blue-900/20 rounded-xl p-6 border border-blue-100 dark:border-blue-900/30">
                            <div className="flex gap-3">
                                <span className="material-symbols-outlined text-blue-600 dark:text-blue-400">contact_support</span>
                                <div>
                                    <p className="text-sm font-bold text-blue-900 dark:text-blue-100">{t('settings.help_title')}</p>
                                    <p className="text-xs text-blue-700 dark:text-blue-300 mt-1 mb-3">{t('settings.help_desc')}</p>
                                    <a className="text-xs font-bold text-blue-600 dark:text-blue-400 hover:underline" href="#">{t('settings.visit_help_center')}</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="h-20"></div> {/* Spacer for scrolling */}
            </div>
        </div>
    );
}
