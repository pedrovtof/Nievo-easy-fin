import React from 'react';
import { Link, useLocation } from 'react-router-dom';

export default function Sidebar() {
    const location = useLocation();
    const navItems = [
        { icon: 'dashboard', label: 'Dashboard', path: '/dashboard' },
        { icon: 'receipt_long', label: 'Transactions', path: '/transactions' },
        { icon: 'donut_large', label: 'Budget', path: '/budget' },
        { icon: 'savings', label: 'Goals', path: '/budget' }, // Mapping goals to budget for now as they share a screen in design
        { icon: 'settings', label: 'Settings', path: '/settings' },
        { icon: 'insights', label: 'Reports', path: '/reports' },
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

            <div className="p-4 border-t border-gray-200">
                <div className="bg-card-light rounded-xl p-4 flex flex-col gap-3">
                    <div className="flex items-center gap-3">
                        <div className="bg-center bg-cover rounded-full size-10 shadow-sm bg-gray-200">
                            {/* Placeholder avatar if image fails to load or just use a color */}
                            <span className="material-symbols-outlined text-gray-400 p-2">person</span>
                        </div>
                        <div className="flex flex-col overflow-hidden">
                            <p className="text-primary text-sm font-bold truncate">Alex Morgan</p>
                            <p className="text-gray-500 text-xs truncate">alex@example.com</p>
                        </div>
                    </div>
                    <button className="w-full py-2 px-3 bg-white border border-gray-200 rounded-lg text-xs font-medium text-gray-600 hover:bg-gray-50 hover:text-primary transition-colors">
                        Settings
                    </button>
                </div>
            </div>
        </aside>
    );
}
