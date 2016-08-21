using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Model
{
	/// <summary>
	/// Очередь исполнения.
	/// </summary>
	/// <typeparam name="T">Тип объекта исполнения.</typeparam>
	public class QueueInvoker<T>
	{
		#region Constants

		/// <summary>
		/// Количество исполнителей по умолчанию.
		/// </summary>
		private const int DEFAULT_QUEUE_COUNT = 1;

		#endregion

		#region Fields

		/// <summary>
		/// Максимальное количество исполнителей.
		/// </summary>
		public int MaxQueueCount { get; set; }

		/// <summary>
		/// Текущее число запущенных исполнителей.
		/// </summary>
		private int _curQueueCount;

		/// <summary>
		/// Запущенны ли сейчас очереди.
		/// </summary>
		public bool IsRun { get; set; }

		/// <summary>
		/// Длина очереди.
		/// </summary>
		public int QueueLength
		{
			get { return _invokeQueue.Count; }
		}

		/// <summary>
		/// Очередь на исполнение.
		/// </summary>
		private readonly ConcurrentQueue<T> _invokeQueue = new ConcurrentQueue<T>();

		/// <summary>
		/// Событие при извлечении из очереди.
		/// </summary>
		public Action<T> OnDequeue;

		/// <summary>
		/// Контролирует запуск и остановку всех очередей.
		/// </summary>
		private readonly ManualResetEvent _runQueueEvent = new ManualResetEvent(false);

		/// <summary>
		/// Синхронизирует работу очередей.
		/// </summary>
		private readonly Semaphore _semaphore = new Semaphore(0, int.MaxValue);

		#endregion

		#region Constructor

		/// <summary>
		/// Инициализирует очередь исполнения исполняемым действием.
		/// </summary>
		/// <param name="action">Исполняемое действие.</param>
		public QueueInvoker(Action<T> action)
			: this(action, DEFAULT_QUEUE_COUNT)
		{

		}

		/// <summary>
		/// Инициализирует очередь исполнения с определенным числом исполнителей.
		/// </summary>
		/// <param name="action">Исполняемое действие.</param>
		/// <param name="queue_count">Число исполнителей.</param>
		public QueueInvoker(Action<T> action, int queue_count)
		{
			MaxQueueCount = queue_count;
			OnDequeue += action;
			RunAllQueueInvoker();
			Run();
		}

		#endregion

		#region Methods / Public
		
		/// <summary>
		/// Добавляет новый объект в очередь на исполнение.
		/// </summary>
		/// <param name="obj">Объект.</param>
		public void Enqueue(T obj)
		{
			_invokeQueue.Enqueue(obj);
			_semaphore.Release();
		}

		/// <summary>
		/// Запускает работу исполнителей.
		/// </summary>
		public void Run()
		{
			IsRun = true;
			_runQueueEvent.Set();
		}

		/// <summary>
		/// Приостанавливает работу всех исполнителей.
		/// </summary>
		public void Stop()
		{
			IsRun = false;
			_runQueueEvent.Reset();
		}

		#endregion

		#region Methods / Private

		/// <summary>
		/// Извлекает объект из очереди и выполняет необходимые действия.
		/// </summary>
		private void Dequeue()
		{
			while (true)
			{
				_runQueueEvent.WaitOne();
				_semaphore.WaitOne();
				T value;
				if (!_invokeQueue.TryDequeue(out value))
				{
					_semaphore.Release();
					Console.WriteLine("QueueInvoker: ошибка при извлечении элемента из очереди.");
					continue;
				}
				try
				{
					OnDequeue.Invoke(value);
				}
				catch (Exception e)
				{
					Console.WriteLine("QueueInvoker: ошибка при обработке элемента в очереди: {0}", e.Message);
				}
			}
		}

		/// <summary>
		/// Запускает требуемое количество потоков исполнения.
		/// </summary>
		private void RunAllQueueInvoker()
		{
			for (; _curQueueCount < MaxQueueCount; _curQueueCount++)
			{
				Task.Run(() => { Dequeue(); });
			}
		}

		#endregion
	}
}
