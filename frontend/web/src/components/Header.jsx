import React from 'react';
import { useLanguage } from '../context/LanguageContext';
import Button from './ui/Button';

export default function Header() {
    const { t } = useLanguage();

    // Mock date for now, could be dynamic
    const dateStr = "October 24, 2023";
    const userName = "Alex";

    // Simple replacement for format strings, proper i18n libs handle this better but t() here is simple lookup
    const greeting = t('header.greeting').replace('{name}', userName);
    const subtitle = t('header.subtitle').replace('{date}', dateStr);

    return (
        <header className="w-full px-8 py-6 bg-white/80 dark:bg-card-dark/80 backdrop-blur-md sticky top-0 z-10 border-b border-gray-100 dark:border-gray-800 flex justify-between items-center transition-colors">
            <div className="flex flex-col gap-1">
                <h2 className="text-2xl font-bold text-primary dark:text-white">{greeting}</h2>
                <p className="text-sm text-gray-500 dark:text-gray-400">{subtitle}</p>
            </div>
            <div className="flex items-center gap-4">
                <button className="relative p-2 text-gray-500 hover:text-primary dark:text-gray-400 dark:hover:text-white transition-colors">
                    <span className="material-symbols-outlined">notifications</span>
                    <span className="absolute top-2 right-2 size-2 bg-accent-red rounded-full"></span>
                </button>
                <Button className="flex items-center justify-center gap-2 px-4 h-10 shadow-lg shadow-primary/20 dark:shadow-none">
                    <span className="material-symbols-outlined text-[18px]">add</span>
                    {t('header.new_transaction')}
                </Button>
            </div>
        </header>
    );
}
