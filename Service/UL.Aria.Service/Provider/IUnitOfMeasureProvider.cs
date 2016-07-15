using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Defines Operations for UnitOfMeasure
    /// </summary>
    public interface IUnitOfMeasureProvider
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<UnitOfMeasure> GetAll();

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>UnitOfMeasure.</returns>
        UnitOfMeasure Fetch(Guid id);

        /// <summary>
        ///     Creates the specified unit of measure.
        /// </summary>
        /// <param name="unitOfMeasure">The unit of measure.</param>
        void Create(UnitOfMeasure unitOfMeasure);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="unitOfMeasure">The unit of measure.</param>
        void Update(Guid id, UnitOfMeasure unitOfMeasure);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(Guid id);
    }

    /// <summary>
    ///     stub
    /// </summary>
    public class UnitOfMeasureProvider : IUnitOfMeasureProvider
    {
        private readonly IUnitOfMeasureRepository _repository;


        /// <summary>
        ///     Initializes a new instance of the <see cref="UnitOfMeasureProvider" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public UnitOfMeasureProvider(IUnitOfMeasureRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UnitOfMeasure> GetAll()
        {
            return _repository.FindAll();
        }

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>UnitOfMeasure.</returns>
        public UnitOfMeasure Fetch(Guid id)
        {
            return _repository.FindById(id);
        }

        /// <summary>
        ///     Creates the specified unit of measure.
        /// </summary>
        /// <param name="unitOfMeasure">The unit of measure.</param>
        public void Create(UnitOfMeasure unitOfMeasure)
        {
            _repository.Add(unitOfMeasure);
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="unitOfMeasure">The unit of measure.</param>
        public void Update(Guid id, UnitOfMeasure unitOfMeasure)
        {
            _repository.Update(unitOfMeasure);
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(Guid id)
        {
            _repository.Remove(id);
        }
    }
}