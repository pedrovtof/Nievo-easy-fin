import React from 'react';

export default function Header() {
    return (
        <header className="w-full px-8 py-6 bg-white/80 backdrop-blur-md sticky top-0 z-10 border-b border-gray-100 flex justify-between items-center">
            <div className="flex flex-col gap-1">
                <h2 className="text-2xl font-bold text-primary">Good morning, Alex</h2>
                <p className="text-sm text-gray-500">Here's your financial overview for October 24, 2023</p>
            </div>
            <div className="flex items-center gap-4">
                <button className="relative p-2 text-gray-500 hover:text-primary transition-colors">
                    <span className="material-symbols-outlined">notifications</span>
                    <span className="absolute top-2 right-2 size-2 bg-accent-red rounded-full"></span>
                </button>
                <button className="flex items-center justify-center h-10 px-4 bg-primary text-white text-sm font-medium rounded-lg shadow-lg shadow-primary/20 hover:bg-primary/90 transition-all cursor-pointer">
                    <span className="material-symbols-outlined text-[18px] mr-2">add</span>
                    New Transaction
                </button>
            </div>
        </header>
    );
}
