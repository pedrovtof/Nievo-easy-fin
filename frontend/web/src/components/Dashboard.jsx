import React from 'react';

export default function Dashboard() {
    return (
        <div className="flex-1 overflow-y-auto p-8">
            <div className="max-w-7xl mx-auto space-y-8 pb-10">
                {/* Summary Stats */}
                <section className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    {/* Total Balance */}
                    <div className="bg-white rounded-xl p-6 border border-gray-100 shadow-sm flex flex-col justify-between h-40 relative overflow-hidden group hover:shadow-md transition-shadow">
                        <div className="absolute right-0 top-0 w-32 h-32 bg-primary/5 rounded-bl-full -mr-8 -mt-8 transition-transform group-hover:scale-110"></div>
                        <div className="flex justify-between items-start z-10">
                            <div>
                                <p className="text-gray-500 text-sm font-medium mb-1">Total Balance</p>
                                <h3 className="text-3xl font-bold text-primary tracking-tight">$2,450.00</h3>
                            </div>
                            <div className="bg-primary/10 text-primary p-2 rounded-lg">
                                <span className="material-symbols-outlined">account_balance</span>
                            </div>
                        </div>
                        <div className="flex items-center gap-2 mt-auto z-10">
                            <span className="bg-accent-green/10 text-accent-green text-xs font-bold px-2 py-1 rounded-full flex items-center gap-1">
                                <span className="material-symbols-outlined text-[14px]">trending_up</span>
                                +12%
                            </span>
                            <span className="text-gray-400 text-xs">vs last month</span>
                        </div>
                    </div>

                    {/* Monthly Income */}
                    <div className="bg-white rounded-xl p-6 border border-gray-100 shadow-sm flex flex-col justify-between h-40 relative overflow-hidden group hover:shadow-md transition-shadow">
                        <div className="absolute right-0 top-0 w-32 h-32 bg-accent-green/5 rounded-bl-full -mr-8 -mt-8 transition-transform group-hover:scale-110"></div>
                        <div className="flex justify-between items-start z-10">
                            <div>
                                <p className="text-gray-500 text-sm font-medium mb-1">Monthly Income</p>
                                <h3 className="text-3xl font-bold text-primary tracking-tight">$4,200.00</h3>
                            </div>
                            <div className="bg-accent-green/10 text-accent-green p-2 rounded-lg">
                                <span className="material-symbols-outlined">payments</span>
                            </div>
                        </div>
                        <div className="flex items-center gap-2 mt-auto z-10">
                            <span className="bg-accent-green/10 text-accent-green text-xs font-bold px-2 py-1 rounded-full flex items-center gap-1">
                                <span className="material-symbols-outlined text-[14px]">arrow_upward</span>
                                +5%
                            </span>
                            <span className="text-gray-400 text-xs">vs last month</span>
                        </div>
                    </div>

                    {/* Monthly Expenses */}
                    <div className="bg-white rounded-xl p-6 border border-gray-100 shadow-sm flex flex-col justify-between h-40 relative overflow-hidden group hover:shadow-md transition-shadow">
                        <div className="absolute right-0 top-0 w-32 h-32 bg-accent-red/5 rounded-bl-full -mr-8 -mt-8 transition-transform group-hover:scale-110"></div>
                        <div className="flex justify-between items-start z-10">
                            <div>
                                <p className="text-gray-500 text-sm font-medium mb-1">Monthly Expenses</p>
                                <h3 className="text-3xl font-bold text-primary tracking-tight">$1,750.00</h3>
                            </div>
                            <div className="bg-accent-red/10 text-accent-red p-2 rounded-lg">
                                <span className="material-symbols-outlined">credit_card</span>
                            </div>
                        </div>
                        <div className="flex items-center gap-2 mt-auto z-10">
                            <span className="bg-accent-orange/10 text-accent-orange text-xs font-bold px-2 py-1 rounded-full flex items-center gap-1">
                                <span className="material-symbols-outlined text-[14px]">arrow_downward</span>
                                -2%
                            </span>
                            <span className="text-gray-400 text-xs">vs last month</span>
                        </div>
                    </div>
                </section>

                {/* Charts & Graphs Placeholders */}
                <section className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                    <div className="lg:col-span-2 bg-white rounded-xl border border-gray-100 p-6 shadow-sm min-h-[300px]">
                        <h3 className="text-lg font-bold text-primary mb-4">Spending Evolution</h3>
                        <div className="w-full h-64 bg-gray-50 rounded flex items-center justify-center text-gray-400">
                            Chart Placeholder
                        </div>
                    </div>
                    <div className="bg-white rounded-xl border border-gray-100 p-6 shadow-sm min-h-[300px]">
                        <h3 className="text-lg font-bold text-primary mb-4">Budget Allocation</h3>
                        <div className="w-full h-64 bg-gray-50 rounded flex items-center justify-center text-gray-400">
                            Donut Chart Placeholder
                        </div>
                    </div>
                </section>
            </div>
        </div>
    );
}
