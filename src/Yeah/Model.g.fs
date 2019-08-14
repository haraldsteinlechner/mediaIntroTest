namespace Yeah.Model

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open Yeah.Model

[<AutoOpen>]
module Mutable =

    
    
    type MModel(__initial : Yeah.Model.Model) =
        inherit obj()
        let mutable __current : Aardvark.Base.Incremental.IModRef<Yeah.Model.Model> = Aardvark.Base.Incremental.EqModRef<Yeah.Model.Model>(__initial) :> Aardvark.Base.Incremental.IModRef<Yeah.Model.Model>
        let _count = ResetMod.Create(__initial.count)
        let _currentModel = ResetMod.Create(__initial.currentModel)
        let _cameraState = Aardvark.UI.Primitives.Mutable.MCameraControllerState.Create(__initial.cameraState)
        
        member x.count = _count :> IMod<_>
        member x.currentModel = _currentModel :> IMod<_>
        member x.cameraState = _cameraState
        
        member x.Current = __current :> IMod<_>
        member x.Update(v : Yeah.Model.Model) =
            if not (System.Object.ReferenceEquals(__current.Value, v)) then
                __current.Value <- v
                
                ResetMod.Update(_count,v.count)
                ResetMod.Update(_currentModel,v.currentModel)
                Aardvark.UI.Primitives.Mutable.MCameraControllerState.Update(_cameraState, v.cameraState)
                
        
        static member Create(__initial : Yeah.Model.Model) : MModel = MModel(__initial)
        static member Update(m : MModel, v : Yeah.Model.Model) = m.Update(v)
        
        override x.ToString() = __current.Value.ToString()
        member x.AsString = sprintf "%A" __current.Value
        interface IUpdatable<Yeah.Model.Model> with
            member x.Update v = x.Update v
    
    
    
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Model =
        [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module Lens =
            let count =
                { new Lens<Yeah.Model.Model, System.Int32>() with
                    override x.Get(r) = r.count
                    override x.Set(r,v) = { r with count = v }
                    override x.Update(r,f) = { r with count = f r.count }
                }
            let currentModel =
                { new Lens<Yeah.Model.Model, Yeah.Model.Primitive>() with
                    override x.Get(r) = r.currentModel
                    override x.Set(r,v) = { r with currentModel = v }
                    override x.Update(r,f) = { r with currentModel = f r.currentModel }
                }
            let cameraState =
                { new Lens<Yeah.Model.Model, Aardvark.UI.Primitives.CameraControllerState>() with
                    override x.Get(r) = r.cameraState
                    override x.Set(r,v) = { r with cameraState = v }
                    override x.Update(r,f) = { r with cameraState = f r.cameraState }
                }
