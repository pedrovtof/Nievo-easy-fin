import React from 'react';

export default function Transactions() {
    const transactions = [
        {
            id: 1,
            date: 'Jun 24, 2023',
            time: '10:42 AM',
            icon: 'sports_esports',
            title: 'Steam Purchase',
            account: 'Debit Card ••4291',
            category: 'Games',
            categoryColor: 'orange',
            amount: -59.99,
            type: 'expense'
        },
        {
            id: 2,
            date: 'Jun 23, 2023',
            time: '04:15 PM',
            icon: 'shopping_cart',
            title: 'Local Market Grocery',
            account: 'Cash Wallet',
            category: 'Market',
            categoryColor: 'blue',
            amount: -120.50,
            type: 'expense'
        },
        {
            id: 3,
            date: 'Jun 22, 2023',
            time: '09:00 AM',
            icon: 'payments',
            title: 'Freelance Payment',
            account: 'Bank Transfer',
            category: 'Income',
            categoryColor: 'green',
            amount: 450.00,
            type: 'income'
        },
        {
            id: 4,
            date: 'Jun 21, 2023',
            time: '06:30 PM',
            icon: 'local_gas_station',
            title: 'Shell Gas Station',
            account: 'Credit Card ••8821',
            category: 'Transport',
            categoryColor: 'purple',
            amount: -45.00,
            type: 'expense'
        },
        {
            id: 5,
            date: 'Jun 20, 2023',
            time: '08:15 AM',
            icon: 'bolt',
            title: 'Electric Bill',
            account: 'Checking ••1102',
            category: 'Utilities',
            categoryColor: 'yellow',
            amount: -110.00,
            type: 'expense'
        },
        {
            id: 6,
            date: 'Jun 19, 2023',
            time: '12:30 PM',
            icon: 'restaurant',
            title: 'Downtown Diner',
            account: 'Credit Card ••8821',
            category: 'Dining',
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

    return (
        <div className="flex-1 overflow-y-auto bg-background-light dark:bg-background-dark p-4 md:p-8 lg:px-12">
            <div className="mx-auto max-w-[1200px] flex flex-col gap-6">
                {/* Page Heading */}
                <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                    <div>
                        <h1 className="text-primary dark:text-white text-3xl md:text-4xl font-black leading-tight tracking-[-0.033em]">Transaction History</h1>
                        <p className="text-[#746f7b] dark:text-[#9e9aa1] text-sm mt-1">Track your spending and income across all accounts.</p>
                    </div>
                    <button className="flex shrink-0 cursor-pointer items-center justify-center gap-2 rounded-lg h-12 px-6 bg-secondary hover:bg-[#1a202c] text-white text-sm font-bold leading-normal tracking-[0.015em] shadow-lg shadow-gray-200 dark:shadow-none transition-all">
                        <span className="material-symbols-outlined text-[20px]">add</span>
                        <span>Add Transaction</span>
                    </button>
                </div>

                {/* Filters Bar */}
                <div className="flex flex-wrap items-center gap-3 p-4 bg-white dark:bg-surface-dark rounded-xl border border-[#e0dfe2] dark:border-[#333] shadow-sm">
                    <div className="flex items-center gap-2 mr-2 text-[#746f7b] dark:text-[#9e9aa1]">
                        <span className="material-symbols-outlined text-[20px]">filter_list</span>
                        <span className="text-sm font-bold uppercase tracking-wider">Filters:</span>
                    </div>
                    {['Period: This Month', 'Account: All Accounts', 'Category: Entertainment', 'Tags: Any'].map((filter, idx) => (
                        <button key={idx} className="flex h-9 items-center justify-center gap-x-2 rounded-lg bg-background-light dark:bg-[#333] border border-transparent hover:border-gray-300 dark:hover:border-gray-600 px-3 transition-all">
                            <span className="text-primary dark:text-white text-sm font-medium">{filter.split(':')[0]}:</span>
                            <span className="text-[#746f7b] dark:text-[#ccc] text-sm">{filter.split(':')[1]}</span>
                            <span className="material-symbols-outlined text-[#746f7b] dark:text-[#ccc] text-[20px]">expand_more</span>
                        </button>
                    ))}
                    <div className="ml-auto pl-2 border-l border-[#e0dfe2] dark:border-[#444]">
                        <button className="text-sm font-bold text-red-600 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300 px-2">
                            Clear
                        </button>
                    </div>
                </div>

                {/* Transactions Table */}
                <div className="rounded-xl border border-[#e0dfe2] dark:border-[#333] bg-white dark:bg-surface-dark overflow-hidden shadow-sm">
                    <div className="overflow-x-auto">
                        <table className="w-full text-left border-collapse">
                            <thead>
                                <tr className="bg-background-light dark:bg-[#1a181d] border-b border-[#e0dfe2] dark:border-[#333]">
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#746f7b] dark:text-[#9e9aa1]">Date</th>
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#746f7b] dark:text-[#9e9aa1] min-w-[200px]">Description</th>
                                    <th className="hidden md:table-cell px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#746f7b] dark:text-[#9e9aa1]">Account</th>
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#746f7b] dark:text-[#9e9aa1]">Category</th>
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#746f7b] dark:text-[#9e9aa1] text-right">Amount</th>
                                    <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#746f7b] dark:text-[#9e9aa1] w-10"></th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-[#e0dfe2] dark:divide-[#333]">
                                {transactions.map((tx) => (
                                    <tr key={tx.id} className="group hover:bg-gray-50 dark:hover:bg-white/5 transition-colors">
                                        <td className="px-6 py-4 whitespace-nowrap">
                                            <div className="flex flex-col">
                                                <span className="text-sm font-bold text-[#141315] dark:text-white">{tx.date}</span>
                                                <span className="text-xs text-[#746f7b] dark:text-[#9e9aa1]">{tx.time}</span>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4">
                                            <div className="flex items-center gap-3">
                                                <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-[#f2f2f3] dark:bg-[#333] text-[#141315] dark:text-white">
                                                    <span className="material-symbols-outlined text-[18px]">{tx.icon}</span>
                                                </div>
                                                <p className="text-sm font-semibold text-[#141315] dark:text-white truncate">{tx.title}</p>
                                            </div>
                                        </td>
                                        <td className="hidden md:table-cell px-6 py-4 text-sm text-[#746f7b] dark:text-[#9e9aa1]">{tx.account}</td>
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
                                            <button className="text-[#746f7b] hover:text-primary dark:text-[#9e9aa1] dark:hover:text-white">
                                                <span className="material-symbols-outlined text-[20px]">more_vert</span>
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                    {/* Footer / Pagination */}
                    <div className="flex items-center justify-between border-t border-[#e0dfe2] dark:border-[#333] px-6 py-4 bg-background-light/50 dark:bg-background-dark/50">
                        <div className="text-sm text-[#746f7b] dark:text-[#9e9aa1]">
                            Showing <span className="font-bold text-[#141315] dark:text-white">1</span> to <span className="font-bold text-[#141315] dark:text-white">6</span> of <span className="font-bold text-[#141315] dark:text-white">128</span> results
                        </div>
                        <div className="flex items-center gap-2">
                            <button className="flex size-8 items-center justify-center rounded-lg border border-[#e0dfe2] dark:border-[#444] bg-white dark:bg-[#232127] hover:bg-gray-50 dark:hover:bg-[#2a282e] disabled:opacity-50">
                                <span className="material-symbols-outlined text-[18px]">chevron_left</span>
                            </button>
                            <button className="flex size-8 items-center justify-center rounded-lg bg-secondary text-white font-bold text-sm">1</button>
                            <button className="flex size-8 items-center justify-center rounded-lg border border-[#e0dfe2] dark:border-[#444] bg-white dark:bg-[#232127] hover:bg-gray-50 dark:hover:bg-[#2a282e] text-sm">2</button>
                            <button className="flex size-8 items-center justify-center rounded-lg border border-[#e0dfe2] dark:border-[#444] bg-white dark:bg-[#232127] hover:bg-gray-50 dark:hover:bg-[#2a282e] text-sm">3</button>
                            <span className="text-[#746f7b] dark:text-[#9e9aa1]">...</span>
                            <button className="flex size-8 items-center justify-center rounded-lg border border-[#e0dfe2] dark:border-[#444] bg-white dark:bg-[#232127] hover:bg-gray-50 dark:hover:bg-[#2a282e] disabled:opacity-50">
                                <span className="material-symbols-outlined text-[18px]">chevron_right</span>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
