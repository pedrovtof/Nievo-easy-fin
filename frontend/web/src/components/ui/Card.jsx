import React from 'react';

export default function Card({ children, className = '', ...props }) {
    return (
        <div
            className={`bg-card-light dark:bg-card-dark rounded-xl p-6 md:p-8 shadow-sm border border-slate-100 dark:border-slate-800 ${className}`}
            {...props}
        >
            {children}
        </div>
    );
}
