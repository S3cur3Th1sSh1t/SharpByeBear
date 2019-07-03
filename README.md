# SharpByeBear

This is a weaponized version for the last Exploit published by SandboxEscaper. 

Most of the code comes from rasta-mouse CollectorService repository (https://github.com/rasta-mouse/CollectorService). I just changed the CVE-2019-0841-Code from the original SandboxEscaper C++ Code to C# and added some checks.

The vulnerability is a race condition in the AppXSVC Service, so you need a target with multiple cores for successfull exploitation.

You can use this executable for exploitation over edge as well as cortana. Just choose the favorite application.

`SharpByeBear.exe license.rtf 1`
`Option1: edge`
`Option2: cortana`

By targeting edge you have to open edge manually after running the executable to trigger the race condition.
For cortana you can just click on the search menu in the lower left.

## CREDITS

- [X] [SandboxEscaper](https://github.com/SandboxEscaper) - Initial Exploit
- [X] [rasta-mouse](https://github.com/rasta-mouse/) - CollectorService Repo
- [X] [RythmStick](https://github.com/RythmStick) - Idea for targeting Cortana instead of Edge

## Legal disclaimer:
Usage of SharpByeBear for attacking targets without prior mutual consent is illegal. It's the end user's responsibility to obey all applicable local, state and federal laws. Developers assume no liability and are not responsible for any misuse or damage caused by this program. Only use for educational / pentesting purposes.
