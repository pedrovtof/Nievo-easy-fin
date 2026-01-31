import React from 'react';

export default function Input({
    label,
    id,
    type = 'text',
    className = '',
    icon,
    rightElement,
    ...props
}) {
    return (
        <div className={`flex flex-col gap-1.5 ${className}`}>
            {label && (
                <label className="text-slate-900 dark:text-slate-200 text-sm font-medium leading-normal" htmlFor={id}>
                    {label}
                </label>
            )}
            <div className="relative flex items-center">
                {icon && (
                    <div className="absolute left-4 text-slate-500 dark:text-slate-400 flex items-center justify-center pointer-events-none">
                        <span className="material-symbols-outlined text-[20px]">{icon}</span>
                    </div>
                )}

                <input
                    className={`form-input flex w-full rounded-lg border border-slate-300 dark:border-slate-600 bg-white dark:bg-slate-800 h-12 ${icon ? 'pl-11' : 'pl-4'} pr-4 text-slate-900 dark:text-white placeholder:text-slate-400 focus:border-primary focus:ring-1 focus:ring-primary transition-colors text-sm font-normal disabled:opacity-50 disabled:cursor-not-allowed`}
                    id={id}
                    type={type}
                    {...props}
                />

                {rightElement && (
                    <div className="absolute right-3">
                        {rightElement}
                    </div>
                )}
            </div>
        </div>
    );
}
