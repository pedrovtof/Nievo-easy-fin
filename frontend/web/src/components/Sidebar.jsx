import React from 'react';
import { Link, useLocation } from 'react-router-dom';

export default function Sidebar() {
    const location = useLocation();
    const navItems = [
        { icon: 'dashboard', label: 'Dashboard', path: '/' },
        { icon: 'receipt_long', label: 'Transactions', path: '/transactions' },
        { icon: 'donut_large', label: 'Budget', path: '/budget' },
        { icon: 'settings', label: 'Settings', path: '/settings' },
    ];

    return (
        <aside className="w-64 flex-shrink-0 flex flex-col bg-white border-r border-gray-200 h-full overflow-y-auto">
            <div className="p-6">
                <div className="flex items-center gap-3">
                    <div className="bg-primary flex items-center justify-center rounded-lg size-10 text-white">
                        <span className="material-symbols-outlined text-2xl">account_balance_wallet</span>
                    </div>
                    <div className="flex flex-col">
                        <h1 className="text-primary text-lg font-bold leading-none">FinControl</h1>
                        <p className="text-gray-500 text-xs font-medium mt-1">Personal Finance</p>
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
                                ? 'bg-primary/10 text-primary group'
                                : 'text-gray-600 hover:bg-gray-100'
                                }`}
                        >
                            <span className={`material-symbols-outlined ${active ? 'filled' : ''}`}>{item.icon}</span>
                            <span className={`text-sm ${active ? 'font-semibold' : 'font-medium'}`}>{item.label}</span>
                        </Link>
                    );
                })}
            </nav>
        </aside>
    );
}
