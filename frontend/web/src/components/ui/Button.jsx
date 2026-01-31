import React from 'react';
import { Link } from 'react-router-dom';

export default function Button({
    children,
    variant = 'primary',
    className = '',
    to,
    ...props
}) {
    const baseStyles = "flex items-center justify-center rounded-lg h-12 font-bold transition-colors shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2";

    const variants = {
        primary: "bg-secondary hover:bg-slate-900 dark:bg-primary dark:hover:bg-blue-600 text-white focus:ring-primary",
        secondary: "bg-white dark:bg-slate-800 border border-slate-300 dark:border-slate-600 text-slate-700 dark:text-slate-200 hover:bg-slate-50 dark:hover:bg-slate-700",
        ghost: "bg-transparent hover:bg-gray-100 dark:hover:bg-gray-800 text-gray-500 dark:text-gray-400",
        danger: "bg-red-50 hover:bg-red-100 text-red-600",
    };

    const combinedClasses = `${baseStyles} ${variants[variant]} ${className}`;

    if (to) {
        return (
            <Link to={to} className={combinedClasses} {...props}>
                {children}
            </Link>
        );
    }

    return (
        <button className={combinedClasses} {...props}>
            {children}
        </button>
    );
}
