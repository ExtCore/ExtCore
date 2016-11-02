// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.Data.Abstractions
{
  /// <summary>
  /// Describes a storage that is implementation of the Unit of Work design pattern with the mechanism
  /// of getting the repositories to work with the underlying storage context and committing the changes
  /// made by all the repositories.
  /// </summary>
  public interface IStorage
  {
    T GetRepository<T>() where T: IRepository;
    void Save();
  }
}