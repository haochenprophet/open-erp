#ifndef BUILD_H
#define BUILD_H

#include "object.h"

namespace n_build {
	class Cbuild :public Object
	{
	public:
		Cbuild();
		int my_init(void *p=nullptr);
	};
}

using namespace n_build;

#endif
