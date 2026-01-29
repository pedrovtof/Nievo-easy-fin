import React from 'react';

export default function Budget() {
    const categories = [
        {
            title: 'Grocery',
            type: 'Essential',
            icon: 'shopping_cart',
            iconClass: 'text-primary dark:text-white',
            amountSpent: 350,
            limit: 400,
            percentage: 87,
            color: 'bg-primary',
            left: 50,
            status: 'normal'
        },
        {
            title: 'Leisure',
            type: 'Discretionary',
            icon: 'theaters',
            iconClass: 'text-accent-red',
            amountSpent: 120,
            limit: 100,
            percentage: 120,
            color: 'bg-accent-red',
            left: -20,
            status: 'over',
            warning: 'Exceeded by $20.00'
        },
        {
            title: 'Personal Care',
            type: 'Essential',
            icon: 'self_care',
            iconClass: 'text-primary dark:text-white',
            amountSpent: 45,
            limit: 100,
            percentage: 45,
            color: 'bg-primary',
            left: 55,
            status: 'normal'
        },
        {
            title: 'Utilities',
            type: 'Recurring',
            icon: 'bolt',
            iconClass: 'text-orange-500',
            amountSpent: 145,
            limit: 150,
            percentage: 96,
            color: 'bg-orange-500',
            left: 5,
            status: 'warning',
            warningText: 'Almost full'
        },
        {
            title: 'Transport',
            type: 'Essential',
            icon: 'directions_car',
            iconClass: 'text-primary dark:text-white',
            amountSpent: 80,
            limit: 200,
            percentage: 40,
            color: 'bg-primary',
            left: 120,
            status: 'normal'
        }
    ];

    return (
        <div className="flex-1 overflow-y-auto p-8">
            <div className="max-w-6xl mx-auto flex flex-col gap-10">
                {/* Summary Stats */}
                <section className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <div className="flex flex-col gap-2 rounded-xl p-6 bg-card-light dark:bg-card-dark border border-transparent dark:border-gray-700 relative overflow-hidden group">
                        <div className="absolute right-0 top-0 h-full w-24 bg-gradient-to-l from-primary/5 to-transparent pointer-events-none"></div>
                        <div className="flex justify-between items-start">
                            <div>
                                <p className="text-gray-600 dark:text-gray-400 text-sm font-semibold uppercase tracking-wide">Total Monthly Budget</p>
                                <p className="text-primary dark:text-white text-4xl font-bold mt-2">$2,400</p>
                            </div>
                            <div className="bg-white dark:bg-gray-800 p-2 rounded-lg shadow-sm">
                                <span className="material-symbols-outlined text-primary dark:text-white">account_balance_wallet</span>
                            </div>
                        </div>
                        <div className="mt-4 flex items-center gap-2">
                            <span className="bg-green-100 text-green-700 text-xs font-bold px-2 py-1 rounded-md flex items-center gap-1">
                                <span className="material-symbols-outlined text-[14px]">trending_flat</span>
                                Stable
                            </span>
                            <p className="text-gray-500 dark:text-gray-400 text-sm">vs last month</p>
                        </div>
                    </div>
                    <div className="flex flex-col gap-2 rounded-xl p-6 bg-card-light dark:bg-card-dark border border-transparent dark:border-gray-700 relative overflow-hidden">
                        <div className="absolute right-0 top-0 h-full w-24 bg-gradient-to-l from-accent-red/5 to-transparent pointer-events-none"></div>
                        <div className="flex justify-between items-start">
                            <div>
                                <p className="text-gray-600 dark:text-gray-400 text-sm font-semibold uppercase tracking-wide">Left to Spend</p>
                                <p className="text-primary dark:text-white text-4xl font-bold mt-2">$850</p>
                            </div>
                            <div className="bg-white dark:bg-gray-800 p-2 rounded-lg shadow-sm">
                                <span className="material-symbols-outlined text-accent-red">savings</span>
                            </div>
                        </div>
                        <div className="mt-4 flex items-center gap-2">
                            <span className="bg-red-100 text-accent-red text-xs font-bold px-2 py-1 rounded-md flex items-center gap-1">
                                <span className="material-symbols-outlined text-[14px]">trending_down</span>
                                -15%
                            </span>
                            <p className="text-gray-500 dark:text-gray-400 text-sm">faster burn rate</p>
                        </div>
                    </div>
                </section>

                {/* Categories Section */}
                <section>
                    <div className="flex items-center justify-between mb-6">
                        <h3 className="text-primary dark:text-white text-xl font-bold">Category Budgets</h3>
                        <div className="flex gap-2">
                            <button className="text-sm font-medium text-gray-500 hover:text-primary dark:hover:text-white flex items-center gap-1 transition-colors">
                                <span className="material-symbols-outlined text-[18px]">filter_list</span> Filter
                            </button>
                            <button className="text-sm font-medium text-gray-500 hover:text-primary dark:hover:text-white flex items-center gap-1 transition-colors">
                                <span className="material-symbols-outlined text-[18px]">sort</span> Sort
                            </button>
                        </div>
                    </div>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {categories.map((cat, idx) => (
                            <div key={idx} className={`bg-card-light dark:bg-card-dark rounded-xl p-6 flex flex-col justify-between min-h-[220px] transition-all hover:shadow-md relative overflow-hidden ${cat.status === 'over' ? 'border border-accent-red/20' : ''}`}>
                                {cat.status === 'over' && (
                                    <div className="absolute top-0 right-0 bg-accent-red text-white text-[10px] font-bold px-3 py-1 rounded-bl-lg flex items-center gap-1">
                                        <span className="material-symbols-outlined text-[12px]">warning</span>
                                        OVER LIMIT
                                    </div>
                                )}
                                <div className="flex justify-between items-start mb-4">
                                    <div className="flex items-center gap-3">
                                        <div className={`bg-white dark:bg-gray-800 rounded-lg p-3 shadow-sm ${cat.iconClass}`}>
                                            <span className="material-symbols-outlined">{cat.icon}</span>
                                        </div>
                                        <div>
                                            <h4 className="font-bold text-lg text-primary dark:text-white">{cat.title}</h4>
                                            <span className="text-xs text-gray-500 dark:text-gray-400 font-medium bg-white dark:bg-gray-800 px-2 py-0.5 rounded-full border border-gray-100 dark:border-gray-700">{cat.type}</span>
                                        </div>
                                    </div>
                                    <button className="text-gray-400 hover:text-primary dark:hover:text-white">
                                        <span className="material-symbols-outlined">more_vert</span>
                                    </button>
                                </div>
                                <div className="mt-auto">
                                    <div className="flex justify-between items-end mb-2">
                                        <div className="flex flex-col">
                                            <span className={`text-3xl font-bold ${cat.status === 'over' ? 'text-accent-red' : 'text-primary dark:text-white'}`}>${cat.amountSpent}</span>
                                            <span className={`text-xs ${cat.status === 'over' ? 'text-accent-red' : 'text-gray-500 dark:text-gray-400'}`}>spent of ${cat.limit} limit</span>
                                        </div>
                                        <span className={`text-lg font-bold ${cat.status === 'over' ? 'text-accent-red' : (cat.status === 'warning' ? 'text-orange-500' : 'text-primary dark:text-white')}`}>{cat.percentage}%</span>
                                    </div>
                                    <div className="w-full bg-white dark:bg-gray-700 rounded-full h-3 overflow-hidden shadow-inner">
                                        <div className={`${cat.color} h-3 rounded-full transition-all duration-500 relative`} style={{ width: `${Math.min(cat.percentage, 100)}%` }}>
                                            {cat.status === 'over' && (
                                                <div className="absolute inset-0 w-full h-full opacity-30" style={{ backgroundImage: 'linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent)', backgroundSize: '1rem 1rem' }}></div>
                                            )}
                                        </div>
                                    </div>
                                    <div className="flex justify-between mt-2">
                                        {cat.status === 'over' ? (
                                            <p className="text-xs text-accent-red font-medium flex items-center gap-1">
                                                <span className="material-symbols-outlined text-[14px]">error</span>
                                                {cat.warning}
                                            </p>
                                        ) : cat.status === 'warning' ? (
                                            <p className="text-xs text-orange-600 font-medium">{cat.warningText}</p>
                                        ) : (
                                            <p className="text-xs text-gray-500">Left: <span className="font-bold text-gray-700 dark:text-gray-300">${cat.left.toFixed(2)}</span></p>
                                        )}
                                    </div>
                                </div>
                            </div>
                        ))}

                        {/* Create New Category Card */}
                        <div className="border-2 border-dashed border-gray-300 dark:border-gray-700 rounded-xl p-6 flex flex-col items-center justify-center min-h-[220px] text-center cursor-pointer hover:bg-gray-50 dark:hover:bg-gray-800/50 hover:border-primary/50 transition-all group">
                            <div className="size-16 rounded-full bg-primary/10 flex items-center justify-center mb-4 group-hover:scale-110 transition-transform">
                                <span className="material-symbols-outlined text-primary dark:text-white text-3xl">add</span>
                            </div>
                            <h4 className="font-bold text-lg text-primary dark:text-white">Create New</h4>
                            <p className="text-sm text-gray-500 dark:text-gray-400 mt-2 max-w-[200px]">Add a new budget category or custom tag</p>
                        </div>
                    </div>
                </section>

                {/* Tags Section */}
                <section className="pb-10">
                    <h3 className="text-primary dark:text-white text-xl font-bold mb-4">Quick Tags</h3>
                    <div className="flex flex-wrap gap-3">
                        {['#SummerVacation/blue-500', '#NewCarFund/green-500', '#SchoolSupplies/purple-500'].map(tag => {
                            const [label, color] = tag.split('/');
                            return (
                                <button key={label} className="flex items-center gap-2 px-4 py-2 rounded-full border border-gray-200 dark:border-gray-700 bg-white dark:bg-card-dark hover:bg-gray-50 dark:hover:bg-gray-800 transition-colors text-sm font-medium text-gray-700 dark:text-gray-300">
                                    <span className={`size-2 rounded-full bg-${color.split('-')[0]}-${color.split('-')[1]}`}></span>
                                    {label}
                                </button>
                            )
                        })}
                        <button className="flex items-center gap-2 px-4 py-2 rounded-full border border-dashed border-gray-300 dark:border-gray-600 bg-transparent hover:bg-gray-50 dark:hover:bg-gray-800 transition-colors text-sm font-medium text-gray-500 dark:text-gray-400">
                            <span className="material-symbols-outlined text-[16px]">add</span>
                            Add Tag
                        </button>
                    </div>
                </section>
            </div>
        </div>
    );
}
