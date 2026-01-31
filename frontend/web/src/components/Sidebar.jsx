import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useLanguage } from '../context/LanguageContext';

export default function Sidebar() {
    const location = useLocation();
    const { t } = useLanguage();

    const navItems = [
        { icon: 'dashboard', label: t('sidebar.dashboard'), path: '/dashboard' },
        { icon: 'receipt_long', label: t('sidebar.transactions'), path: '/transactions' },
        { icon: 'donut_large', label: t('sidebar.budget'), path: '/budget' },
        { icon: 'savings', label: t('sidebar.goals'), path: '/budget' }, // Mapping goals to budget for now as they share a screen in design
        { icon: 'settings', label: t('sidebar.settings'), path: '/settings' },
        { icon: 'insights', label: t('sidebar.reports'), path: '/reports' },
    ];

    return (
        <aside className="w-64 flex-shrink-0 flex flex-col bg-white dark:bg-card-dark border-r border-gray-200 dark:border-gray-700 h-full overflow-y-auto transition-colors">
            <div className="p-6">
                <div className="flex items-center gap-3">
                    <div className="bg-primary flex items-center justify-center rounded-lg size-10 text-white">
                        <span className="material-symbols-outlined text-2xl">account_balance_wallet</span>
                    </div>
                    <div className="flex flex-col">
                        <h1 className="text-primary dark:text-white text-lg font-bold leading-none">{t('common.app_name')}</h1>
                        <p className="text-gray-500 dark:text-gray-400 text-xs font-medium mt-1">{t('common.app_subtitle')}</p>
                    </div>
                </div>
            </div>

            <nav className="flex-1 px-4 py-2 space-y-1">
                {navItems.map((item) => {
                    const active = location.pathname === item.path;
                    return (
                        <Link
                            key={item.label}
                            to={item.path}
                            className={`flex items-center gap-3 px-3 py-3 rounded-lg transition-colors ${active
                                ? 'bg-primary/10 text-primary dark:text-white group'
                                : 'text-gray-600 dark:text-gray-400 hover:bg-gray-100 dark:hover:bg-gray-800'
                                }`}
                        >
                            <span className={`material-symbols-outlined ${active ? 'filled' : ''}`}>{item.icon}</span>
                            <span className={`text-sm ${active ? 'font-semibold' : 'font-medium'}`}>{item.label}</span>
                        </Link>
                    );
                })}
            </nav>

            <div className="p-4 border-t border-gray-200 dark:border-gray-700">
                <div className="bg-card-light dark:bg-gray-800 rounded-xl p-4 flex flex-col gap-3">
                    <div className="flex items-center gap-3">
                        <div className="bg-center bg-cover rounded-full size-10 shadow-sm bg-gray-200 dark:bg-gray-700 flex items-center justify-center">
                            {/* Placeholder avatar if image fails to load or just use a color */}
                            <span className="material-symbols-outlined text-gray-400 dark:text-gray-300 p-2">person</span>
                        </div>
                        <div className="flex flex-col overflow-hidden">
                            <p className="text-primary dark:text-white text-sm font-bold truncate">Alex Morgan</p>
                            <p className="text-gray-500 dark:text-gray-400 text-xs truncate">alex@example.com</p>
                        </div>
                    </div>
                    <Link to="/settings" className="w-full flex justify-center py-2 px-3 bg-white dark:bg-card-dark border border-gray-200 dark:border-gray-600 rounded-lg text-xs font-medium text-gray-600 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700 hover:text-primary dark:hover:text-white transition-colors">
                        {t('sidebar.profile_settings')}
                    </Link>
                </div>
            </div>
        </aside>
    );
}
