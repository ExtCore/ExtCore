// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.Data.Abstractions
{
  public interface IStorage
  {
    T GetRepository<T>() where T: IRepository;
    void Save();
  }
}