import React from 'react';
import Sidebar from './Sidebar';
import Header from './Header';

export default function Layout({ children }) {
    return (
        <div className="bg-background-light dark:bg-background-dark font-display text-secondary antialiased overflow-hidden">
            <div className="flex h-screen w-full overflow-hidden">
                <Sidebar />
                <main className="flex-1 flex flex-col h-full overflow-hidden relative">
                    <Header />
                    {children}
                </main>
            </div>
        </div>
    );
}
