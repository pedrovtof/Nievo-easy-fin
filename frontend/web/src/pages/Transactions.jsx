import React from 'react';
import { useLanguage } from '../context/LanguageContext';
import Button from '../components/ui/Button';

export default function Transactions() {
    const { t } = useLanguage();

    const transactions = [
        {
            id: 1,
            date: 'Jun 24, 2023',
            time: '10:42 AM',
            icon: 'sports_esports',
            title: t('transactions.items.steam'),
            account: 'Debit Card ••4291',
            category: t('transactions.categories.games'),
            categoryColor: 'orange',
            amount: -59.99,
            type: 'expense'
        },
        {
            id: 2,
            date: 'Jun 23, 2023',
            time: '04:15 PM',
            icon: 'shopping_cart',
            title: t('transactions.items.market'),
            account: 'Cash Wallet',
            category: t('transactions.categories.market'),
            categoryColor: 'blue',
            amount: -120.50,
            type: 'expense'
        },
        {
            id: 3,
            date: 'Jun 22, 2023',
            time: '09:00 AM',
            icon: 'payments',
            title: t('transactions.items.freelance'),
            account: 'Bank Transfer',
            category: t('transactions.categories.income'),
            categoryColor: 'green',
            amount: 450.00,
            type: 'income'
        },
        {
            id: 4,
            date: 'Jun 21, 2023',
            time: '06:30 PM',
            icon: 'local_gas_station',
            title: t('transactions.items.shell'),
            account: 'Credit Card ••8821',
            category: t('transactions.categories.transport'),
            categoryColor: 'purple',
            amount: -45.00,
            type: 'expense'
        },
        {
            id: 5,
            date: 'Jun 20, 2023',
            time: '08:15 AM',
            icon: 'bolt',
            title: t('transactions.items.electric'),
            account: 'Checking ••1102',
            category: t('transactions.categories.utilities'),
            categoryColor: 'yellow',
            amount: -110.00,
            type: 'expense'
        },
        {
            id: 6,
            date: 'Jun 19, 2023',
            time: '12:30 PM',
            icon: 'restaurant',
            title: t('transactions.items.diner'),
            account: 'Credit Card ••8821',
            category: t('transactions.categories.dining'),
            categoryColor: 'pink',
            amount: -35.20,
            type: 'expense'
        }
    ];

    const getCategoryStyles = (color) => {
        const colors = {
            orange: 'bg-orange-100 dark:bg-orange-900/30 text-orange-800 dark:text-orange-300',
            blue: 'bg-blue-100 dark:bg-blue-900/30 text-blue-800 dark:text-blue-300',
            green: 'bg-green-100 dark:bg-green-900/30 text-green-800 dark:text-green-300',
            purple: 'bg-purple-100 dark:bg-purple-900/30 text-purple-800 dark:text-purple-300',
            yellow: 'bg-yellow-100 dark:bg-yellow-900/30 text-yellow-800 dark:text-yellow-300',
            pink: 'bg-pink-100 dark:bg-pink-900/30 text-pink-800 dark:text-pink-300',
        };
        return colors[color] || colors.blue;
    };

    const filters = [
        `${t('transactions.filter_period')}: ${t('transactions.filters.period_this_month')}`,
        `${t('transactions.filter_account')}: ${t('transactions.filters.account_all')}`,
        `${t('transactions.filter_category')}: ${t('transactions.filters.cat_entertainment')}`,
        `${t('transactions.filter_tags')}: ${t('transactions.filters.tags_any')}`
    ];

    return (
        <div className="flex-1 overflow-y-auto bg-background-light dark:bg-background-dark p-4 md:p-8 lg:px-12">
            <div className="mx-auto max-w-[1200px] flex flex-col gap-6">
                {/* Page Heading */}
                <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                    <div>
                        <h1 className="text-primary dark:text-white text-3xl md:text-4xl font-black leading-tight tracking-[-0.033em]">{t('transactions.title')}</h1>
                        <p className="text-slate-500 dark:text-slate-400 text-sm mt-1">{t('transactions.subtitle')}</p>
                    </div>
                    <Button className="flex items-center justify-center gap-2 px-6 shadow-lg shadow-gray-200 dark:shadow-none transition-all">
                        <span className="material-symbols-outlined text-[20px]">add</span>
                        <span>{t('transactions.add_button')}</span>
                    </Button>
                </div>

                {/* Filters Bar */}
                <div className="flex flex-wrap items-center gap-3 p-4 bg-white dark:bg-surface-dark rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm">
                    <div className="flex items-center gap-2 mr-2 text-slate-500 dark:text-slate-400">
                        <span className="material-symbols-outlined text-[20px]">filter_list</span>
                        <span className="text-sm font-bold uppercase tracking-wider">{t('transactions.filters_label')}</span>
                    </div>
                    {filters.map((filter, idx) => (
                        <button key={idx} className="flex h-9 items-center justify-center gap-x-2 rounded-lg bg-background-light dark:bg-slate-800 border border-transparent hover:border-gray-300 dark:hover:border-gray-600 px-3 transition-all">
                            <span className="text-primary dark:text-white text-sm font-medium">{filter.split(':')[0]}:</span>
                            <span className="text-slate-500 dark:text-slate-400 text-sm">{filter.split(':')[1]}</span>
                            <span className="material-symbols-outlined text-slate-500 dark:text-slate-400 text-[20px]">expand_more</span>
                        </button>
                    ))}
                    <div className="ml-auto pl-2 border-l border-slate-200 dark:border-slate-800">
                        <button className="text-sm font-bold text-red-600 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300 px-2">
                            {t('transactions.clear_filters')}
                        </button>
                    </div>
                </div>

                {/* Transactions Table */}
                <div className="rounded-xl border border-slate-200 dark:border-slate-800 bg-white dark:bg-card-dark overflow-hidden shadow-sm">
                    <div className="overflow-x-auto">
                        <table className="w-full text-left border-collapse">
                            <thead>
                                <tr className="bg-slate-50 dark:bg-slate-900 border-b border-slate-200 dark:border-slate-800">
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400">{t('transactions.th_date')}</th>
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400 min-w-[200px]">{t('transactions.th_description')}</th>
                                    <th className="hidden md:table-cell px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400">{t('transactions.th_account')}</th>
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400">{t('transactions.th_category')}</th>
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400 text-right">{t('transactions.th_amount')}</th>
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400 w-10"></th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-slate-200 dark:divide-slate-800">
                                {transactions.map((tx) => (
                                    <tr key={tx.id} className="group hover:bg-slate-50 dark:hover:bg-white/5 transition-colors">
                                        <td className="px-6 py-4 whitespace-nowrap">
                                            <div className="flex flex-col">
                                                <span className="text-sm font-bold text-slate-900 dark:text-white">{tx.date}</span>
                                                <span className="text-xs text-slate-500 dark:text-slate-400">{tx.time}</span>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4">
                                            <div className="flex items-center gap-3">
                                                <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-slate-100 dark:bg-slate-800 text-slate-900 dark:text-white">
                                                    <span className="material-symbols-outlined text-[18px]">{tx.icon}</span>
                                                </div>
                                                <p className="text-sm font-semibold text-slate-900 dark:text-white truncate">{tx.title}</p>
                                            </div>
                                        </td>
                                        <td className="hidden md:table-cell px-6 py-4 text-sm text-slate-500 dark:text-slate-400">{tx.account}</td>
                                        <td className="px-6 py-4">
                                            <span className={`inline-flex items-center rounded-md px-2.5 py-0.5 text-xs font-bold ${getCategoryStyles(tx.categoryColor)}`}>
                                                {tx.category}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 text-right">
                                            <span className={`text-sm font-bold ${tx.type === 'income' ? 'text-green-600 dark:text-green-400' : 'text-red-600 dark:text-red-400'}`}>
                                                {tx.type === 'expense' ? '-' : '+'}${Math.abs(tx.amount).toFixed(2)}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 text-center">
                                            <button className="text-slate-400 hover:text-primary dark:text-slate-400 dark:hover:text-white">
                                                <span className="material-symbols-outlined text-[20px]">more_vert</span>
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                    {/* Footer / Pagination */}
                    <div className="flex items-center justify-between border-t border-slate-200 dark:border-slate-800 px-6 py-4 bg-background-light/50 dark:bg-background-dark/50">
                        <div className="text-sm text-slate-500 dark:text-slate-400">
                            {t('transactions.footer_showing')} <span className="font-bold text-slate-900 dark:text-white">1</span> {t('transactions.footer_to')} <span className="font-bold text-slate-900 dark:text-white">6</span> {t('transactions.footer_of')} <span className="font-bold text-slate-900 dark:text-white">128</span> {t('transactions.footer_results')}
                        </div>
                        <div className="flex items-center gap-2">
                            <button className="flex size-8 items-center justify-center rounded-lg border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 hover:bg-slate-50 dark:hover:bg-slate-700 disabled:opacity-50 text-slate-500 dark:text-white">
                                <span className="material-symbols-outlined text-[18px]">chevron_left</span>
                            </button>
                            <button className="flex size-8 items-center justify-center rounded-lg bg-secondary text-white font-bold text-sm">1</button>
                            <button className="flex size-8 items-center justify-center rounded-lg border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 hover:bg-slate-50 dark:hover:bg-slate-700 text-sm text-slate-500 dark:text-white">2</button>
                            <button className="flex size-8 items-center justify-center rounded-lg border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 hover:bg-slate-50 dark:hover:bg-slate-700 text-sm text-slate-500 dark:text-white">3</button>
                            <span className="text-slate-500 dark:text-slate-400">...</span>
                            <button className="flex size-8 items-center justify-center rounded-lg border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 hover:bg-slate-50 dark:hover:bg-slate-700 disabled:opacity-50 text-slate-500 dark:text-white">
                                <span className="material-symbols-outlined text-[18px]">chevron_right</span>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
