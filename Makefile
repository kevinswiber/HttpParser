CC = gcc
CSBUILD = xbuild
CPPFLAGS = -m32
HTTP_PARSER_DIR = ./deps/http-parser
HTTP_PARSER_FLAGS = -C $(HTTP_PARSER_DIR) CPPFLAGS_FAST_EXTRA=$(CPPFLAGS) 
STATIC_LIBRARY = DYLIB

DYLIB = -dynamiclib -framework CoreServices

all:
	mkdir ./build
	make $(HTTP_PARSER_FLAGS) libhttp_parser.o
	gcc $($(STATIC_LIBRARY)) $(CPPFLAGS) -o ./build/libhttp_parser.dylib ./deps/http-parser/libhttp_parser.o
	$(CSBUILD) ./src/HttpParser.sln

clean:
	rm -rf ./build
	make -C ./deps/http-parser clean
